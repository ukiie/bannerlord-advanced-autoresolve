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
            if (SubModule.IsValidEventType(battle.EventType) && SimulationsPool.TryRemoveSimulationModel(battle.Id, out var model))
            {
                if (model.IsPlayerInvolved)
                {
                    MessageHelper.DisplayText($"Finished {model.EventDescription}", DisplayTextStyle.Info);
                }
            }
        }

        private void Initialize(MapEvent battle, PartyBase attackerParty, PartyBase defenderParty)
        {
            if (SubModule.IsValidEventType(battle.EventType))
            {
                var model = new SimulationModel(battle, attackerParty, defenderParty);
                SimulationsPool.AddModelToSimulations(model);
                if (model.IsPlayerInvolved)
                {
                    MessageHelper.DisplayText($"Initialized {model.EventDescription}", DisplayTextStyle.Info);
                }
                else
                {
                    // battle observer for battles started by AI
                    battle.AddSimulationObserver();
                }
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
    }
}
