using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace AdvancedAutoResolve.HarmonyPatches.MapEventSidePatches
{
    [HarmonyPatch]
    internal static class MapEventSideReversePatches
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(MapEventSide), nameof(RemoveSelectedTroopFromSimulationList))]
        internal static void RemoveSelectedTroopFromSimulationList(this MapEventSide __instance)
        {
            throw new NotImplementedException("Reverse patch didn't work");
        }
    }
}
