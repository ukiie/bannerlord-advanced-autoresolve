using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AdvancedAutoResolve.Helpers
{
    internal static class MapEventExtensions
    {
        internal static void AddSimulationObserver(this MapEvent mapEvent)
        {
            var prop = AccessTools.DeclaredProperty(typeof(MapEvent), "BattleObserver");
            prop.SetValue(mapEvent, new SimulationObserver());
        }
    }
}
