using AdvancedAutoResolve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAutoResolve.Helpers
{
    internal static class TroopTypeExtensions
    {
        internal static bool IsInfantryTroopType(this int index)
        {
            return 
                index == (int)TroopType.ShockInfantry || 
                index == (int)TroopType.SkirmishInfantry || 
                index == (int)TroopType.HeavyInfantry;
        }

        internal static bool IsRangedTroopType(this int index)
        {
            return 
                index == (int)TroopType.Ranged;
        }

        internal static bool IsCavalryTroopType(this int index)
        {

            return 
                index == (int)TroopType.LightCavalry || 
                index == (int)TroopType.HeavyCavalry;
        }
        internal static bool IsHorseArcherTroopType(this int index)
        {
            return
                index == (int)TroopType.HorseArcher;
        }
    }
}
