using AdvancedAutoResolve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AdvancedAutoResolve.Helpers
{
    internal static class CharacterObjectExtensions
    {
        internal static TroopType DecideTroopType(this CharacterObject troop)
        {
            var type = TroopType.ShockInfantry;
            try
            {
                bool hasThrowables = false;
                bool hasRanged = false;
                bool hasShield = false;

                for (int i = 0; i < 4; i++)
                {
                    if (troop.Equipment != null)
                    {
                        var item = troop.Equipment[(EquipmentIndex)i];
                        if (!item.IsEmpty)
                        {
                            if (item.Item != null)
                            {
                                if (item.Item.Type == ItemObject.ItemTypeEnum.Shield)
                                    hasShield = true;
                                if (item.Item.Type == ItemObject.ItemTypeEnum.Thrown)
                                    hasThrowables = true;
                                if (item.Item.Type == ItemObject.ItemTypeEnum.Crossbow || item.Item.Type == ItemObject.ItemTypeEnum.Bow)
                                    hasRanged = true;
                            }
                        }
                    }
                }
                var sumOfAllArmor = troop.GetArmArmorSum() + troop.GetBodyArmorSum() + troop.GetHeadArmorSum() + troop.GetLegArmorSum();
                var horseArmor = troop.GetHorseArmorSum();


                if (hasRanged && !troop.IsMounted)
                    return TroopType.Ranged;

                if (hasRanged && troop.IsMounted)
                    return TroopType.HorseArcher;

                if (!hasRanged && !troop.IsMounted && hasThrowables && !hasShield && sumOfAllArmor < 75)
                    return TroopType.SkirmishInfantry;

                if (!hasRanged && !troop.IsMounted && !hasShield)
                    return TroopType.ShockInfantry;

                if (!hasRanged && !troop.IsMounted && hasShield)
                    return TroopType.HeavyInfantry;

                if (!hasRanged && troop.IsMounted && (sumOfAllArmor > 75 || horseArmor > 40))
                    return TroopType.HeavyCavalry;

                if (!hasRanged && troop.IsMounted && (sumOfAllArmor < 75 || horseArmor < 40))
                    return TroopType.LightCavalry;
            }
            catch
            {
            }
            return type;
        }
    }
}
