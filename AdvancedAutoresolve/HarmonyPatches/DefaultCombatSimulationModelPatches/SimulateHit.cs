using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Simulation;
using AdvancedAutoResolve.Simulation.Models;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;

namespace AdvancedAutoResolve.HarmonyPatches.DefaultCombatSimulationModelPatches
{
    [HarmonyPatch(typeof(DefaultCombatSimulationModel), nameof(DefaultCombatSimulationModel.SimulateHit))]
    internal static class SimulateHit
    {
        private static bool Prefix(ref int __result, ref DefaultCombatSimulationModel __instance, CharacterObject strikerTroop, CharacterObject strikedTroop, PartyBase strikerParty, PartyBase strikedParty, float strikerAdvantage, MapEvent battle)
        {
            if (SimulationsPool.TryGetSimulationModel(battle.Id, out SimulationModel simulationModel))
            {
                try
                {
                    __result = simulationModel.SimulateHit(strikerTroop.Id, strikedTroop.Id, strikerAdvantage);
                    return false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageHelper.DisplayText($"{simulationModel.BattleId} Critical error simulating hit: " + ex.Message, DisplayTextStyle.Warning);
#endif
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
