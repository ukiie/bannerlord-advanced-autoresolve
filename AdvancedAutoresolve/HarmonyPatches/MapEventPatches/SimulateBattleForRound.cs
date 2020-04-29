using AdvancedAutoResolve.Simulation;
using AdvancedAutoResolve.Simulation.Models;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation.Tags;
using TaleWorlds.Core;

namespace AdvancedAutoResolve.HarmonyPatches.MapEventPatches
{
    [HarmonyPatch(typeof(MapEvent), "SimulateBattleForRound")]
    internal static class SimulateBattleForRound
    {
        internal static void Postfix(ref MapEvent __instance, BattleSideEnum side, float advantage)
        {
            if (SimulationsPool.TryGetSimulationModel(__instance.Id, out SimulationModel simulationModel))
            {
                simulationModel.ChangeTactics(side);
            }
        }
    }
}
