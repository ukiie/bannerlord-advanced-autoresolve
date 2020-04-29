using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Simulation;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace AdvancedAutoResolve.HarmonyPatches.MapEventPatches
{
    [HarmonyPatch(typeof(MapEvent), nameof(MapEvent.FinalizeEvent))]
    internal static class FinalizeEvent
    {
        internal static void Postfix(ref MapEvent __instance)
        {
            if (SubModule.IsValidEventType(__instance.EventType) && SimulationsPool.TryRemoveSimulationModel(__instance.Id, out var model))
            {
#if DEBUG
                MessageHelper.DisplayText($"Finished {model.EventDescription}", DisplayTextStyle.Info);
#endif
            }

        }
    }
}
