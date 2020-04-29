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
    internal class SimulationObserver : IBattleObserver
    {
        public void BattleResultsReady()
        {
        }

        public void HeroSkillIncreased(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject heroCharacter, SkillObject skill)
        {
        }

        public void TroopNumberChanged(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int number = 0, int numberKilled = 0, int numberWounded = 0, int numberRouted = 0, int killCount = 0, int numberReadyToUpgrade = 0)
        {
            if(battleCombatant is PartyBase party)
            {
                if(party.MapEvent != null && SubModule.IsValidEventType(party.MapEvent.EventType))
                {
                    if(SimulationsPool.TryGetSimulationModel(party.MapEvent.Id, out SimulationModel simulationModel))
                    {
                        simulationModel.RemoveTroop(side, character.Id);
                    }
                }
            }
        }
    }
}
