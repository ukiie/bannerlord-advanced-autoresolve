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
        private static bool Prefix(ref int __result, CharacterObject strikerTroop, CharacterObject strikedTroop, PartyBase strikerParty, PartyBase strikedParty, float strikerAdvantage, MapEvent battle)
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
                    if (Config.CurrentConfig.ShouldLogThis(simulationModel.IsPlayerInvolved))
                    {
                        MessageHelper.DisplayText($"{simulationModel.EventDescription} Critical error simulating hit: " + ex.Message, DisplayTextStyle.Warning);
                    }

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
