using AdvancedAutoResolve.Configuration;
using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Simulation;
using AdvancedAutoResolve.Simulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AdvancedAutoResolve
{
    internal class AdvancedAutoResolveLogic : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(Initialize));
            CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(Finalize));
        }

        private void Finalize(MapEvent battle)
        {
            if (SimulationModel.IsValidEventType(battle.EventType) && SimulationsPool.TryRemoveSimulationModel(battle.Id, out var simulationModel))
            {

                if (Config.CurrentConfig.ShouldLogThis(simulationModel.IsPlayerInvolved))
                {
                    MessageHelper.DisplayText($"Finished {simulationModel.EventDescription}", DisplayTextStyle.Info);
                }
            }
        }

        private void Initialize(MapEvent battle, PartyBase attackerParty, PartyBase defenderParty)
        {
            if (SimulationModel.IsValidEventType(battle.EventType))
            {
                var simulationModel = new SimulationModel(battle, attackerParty, defenderParty);
                
                if (!simulationModel.IsPlayerInvolved && !Config.CurrentConfig.EnabledForAI)
                    return;

                if (SimulationsPool.AddModelToSimulations(simulationModel))
                {
                    if (simulationModel.IsPlayerInvolved)
                    {
                        if (Config.CurrentConfig.ShouldLogThis(simulationModel.IsPlayerInvolved))
                        {
                            MessageHelper.DisplayText($"Initialized {simulationModel.EventDescription}", DisplayTextStyle.Info);
                        }
                    }
                    else
                    {
                        // battle observer for battles started by AI
                        battle.AddSimulationObserver();
                    }
                }
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
    }
}
