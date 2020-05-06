using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using static HarmonyLib.AccessTools;

namespace AdvancedAutoResolve.HarmonyPatches.MapEventSidePatches
{
    internal static class MapEventSideAccessTools
    {
        private static readonly PropertyInfo _battleObserverPropertyGetter = Property(typeof(MapEventSide), "BattleObserver");
        private static readonly FieldRef<MapEventSide, Dictionary<UniqueTroopDescriptor, MapEventParty>> _allocatedTroops = FieldRefAccess<MapEventSide, Dictionary<UniqueTroopDescriptor, MapEventParty>>("AllocatedTroops");

        internal static IBattleObserver GetBattleObserver(MapEventSide mapEventSide) => (IBattleObserver)_battleObserverPropertyGetter.GetValue(mapEventSide);
        internal static Dictionary<UniqueTroopDescriptor, MapEventParty> GetAllocatedTroops(MapEventSide mapEventSide) => _allocatedTroops(mapEventSide);
    }
}
