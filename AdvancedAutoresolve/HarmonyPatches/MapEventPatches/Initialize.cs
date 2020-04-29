using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Simulation;
using AdvancedAutoResolve.Simulation.Models;
using HarmonyLib;
using Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace AdvancedAutoResolve.HarmonyPatches.MapEventPatches
{
    [HarmonyPatch(typeof(MapEvent), nameof(MapEvent.Initialize))]
    internal static class Initialize
    {
        private static void Postfix(ref MapEvent __instance, PartyBase attackerParty, PartyBase defenderParty, MapEvent.BattleTypes mapEventType = MapEvent.BattleTypes.None)
        {
            if(SubModule.IsValidEventType(mapEventType))
            {
                var model = new SimulationModel(__instance, attackerParty, defenderParty);
                SimulationsPool.AddModelToSimulations(model);
#if DEBUG
                MessageHelper.DisplayText($"Initialized {model.EventDescription}", DisplayTextStyle.Info);
#endif
            }
        }
    }
}
