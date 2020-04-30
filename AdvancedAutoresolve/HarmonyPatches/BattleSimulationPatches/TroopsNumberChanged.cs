using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AdvancedAutoResolve.HarmonyPatches.BattleSimulationPatches
{
    [HarmonyPatch(typeof(BattleSimulation), nameof(BattleSimulation.TroopNumberChanged))]
    internal static class TroopsNumberChanged
    {
        internal static void Postfix(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int number = 0, int numberKilled = 0, int numberWounded = 0, int numberRouted = 0, int killCount = 0, int numberReadyToUpgrade = 0)
        {
            SimulationObserver.TroopNumberChangedInternal(side, battleCombatant, character, number, numberKilled, numberWounded, numberRouted, killCount, numberReadyToUpgrade);
        }
    }
}
