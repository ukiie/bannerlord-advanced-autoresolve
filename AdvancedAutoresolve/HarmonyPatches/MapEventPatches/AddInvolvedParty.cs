using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Simulation;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AdvancedAutoResolve.HarmonyPatches.MapEventPatches
{
    [HarmonyPatch(typeof(MapEvent), nameof(MapEvent.AddInvolvedParty))]
    internal static class AddInvolvedParty
    {
        internal static void Postfix(ref MapEvent __instance, PartyBase involvedParty, BattleSideEnum side, bool notFromInit = true)
        {
            if(SimulationsPool.TryGetSimulationModel(__instance.Id, out var simulationModel) && notFromInit)
            {
                simulationModel.AddTroopsFromInvolvedParty(involvedParty, side);

                if (Config.CurrentConfig.ShouldLogThis(simulationModel.IsPlayerInvolved))
                {
                    MessageHelper.DisplayText($"{involvedParty} joined {simulationModel.EventDescription}", DisplayTextStyle.Info);
                }
            }
        }
    }
}
