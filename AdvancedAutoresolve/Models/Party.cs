using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Simulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AdvancedAutoResolve.Models
{
    internal class Party
    {
        internal bool HasLeader { get; }

        internal TerrainType TerrainType { get; }

        //TODO Change this or add separately all Heros from parties to give exta bonuses to troops from perks
        internal PartyLeader PartyLeader { get; }
        internal InfantryTactics CurrentInfantryTactic { get; private set; } = InfantryTactics.NoTactic;
        internal RangedTactics CurrentArchersTactic { get; private set; } = RangedTactics.NoTactic;
        internal CavalryTactics CurrentCavalryTactic { get; private set; } = CavalryTactics.NoTactic;
        internal HorseArchersTactics CurrentHorseArchersTactic { get; private set; } = HorseArchersTactics.NoTactic;

        internal List<Troop> Troops { get; }

        internal bool HasInfantry => Troops.Any(t => TroopTypeExtensions.IsInfantryTroopType((int)t.TroopType));
        internal bool HasArchers => Troops.Any(t => TroopTypeExtensions.IsRangedTroopType((int)t.TroopType));
        internal bool HasCavalry => Troops.Any(t => TroopTypeExtensions.IsCavalryTroopType((int)t.TroopType));
        internal bool HasHorseArchers => Troops.Any(t => TroopTypeExtensions.IsHorseArcherTroopType((int)t.TroopType));


        internal Party(PartyBase party)
        {
            TerrainType = Campaign.Current.MapSceneWrapper.GetTerrainTypeAtPosition(party.MapEvent.Position);
            HasLeader = party.LeaderHero != null && party.LeaderHero.HitPoints > 20; // leader hero is present and not wounded
            if (HasLeader)
            {
                PartyLeader = new PartyLeader(
                    party.LeaderHero.GetSkillValue(DefaultSkills.Leadership),
                    party.LeaderHero.GetSkillValue(DefaultSkills.Tactics),
                    party.LeaderHero.GetPerkValue(DefaultPerks.Tactics.TacticalSuperiority),
                    party.LeaderHero.GetPerkValue(DefaultPerks.Tactics.HammerAndAnvil),
                    party.LeaderHero.GetPerkValue(DefaultPerks.Tactics.Phalanx),
                    party.LeaderHero.GetPerkValue(DefaultPerks.Tactics.AmbushSpecialist));
            }

            Troops = new List<Troop>();

            AddTroopsFromParty(party);

            SelectBattlesTactics();
        }

        internal void RemoveTroop(MBGUID troopId)
        {
            Troops.Remove(Troops.Find(t => t.CharacterObject.Id == troopId));
        }

        internal void AddTroopsFromParty(PartyBase party)
        {
            var troops = party.MemberRoster;
            foreach (var troop in troops)
            {
                int totalNumber = troop.Number - troop.WoundedNumber;
                var troopType = DecideTroopType(troop.Character);
                var testTroopType = DecideTroopTypeTest(troop.Character);

                while (totalNumber-- > 0)
                {
                    Troops.Add(new Troop(troop.Character, this, troopType, testTroopType));
                }
            }
            if (party.MobileParty != null && party.MobileParty.AttachedParties != null)
            {
                foreach (var attachedParty in party.MobileParty.AttachedParties)
                {
                    AddTroopsFromParty(attachedParty.Party);
                }
            }
        }

        private TroopType DecideTroopType(CharacterObject troop)
        {
            var type = TroopType.HeavyInfantry;
            if (troop.CurrentFormationClass == FormationClass.Infantry)
                type = TroopType.HeavyInfantry;
            if (troop.CurrentFormationClass == FormationClass.Ranged)
                type = TroopType.Ranged;
            if (troop.CurrentFormationClass == FormationClass.Cavalry)
                type = TroopType.LightCavalry;
            if (troop.CurrentFormationClass == FormationClass.HorseArcher)
                type = TroopType.HorseArcher;

            return type;
        }

        private TroopType DecideTroopTypeTest(CharacterObject troop)
        {
            bool hasThrowables = false;
            bool hasRanged = false;
            bool hasShield = false;

            for (int i = 0; i < 4; i++)
            {
                var item = troop.Equipment[(EquipmentIndex)i];
                if (!item.IsEmpty)
                {
                    if (item.Item.Type == ItemObject.ItemTypeEnum.Shield)
                        hasShield = true;
                    if (item.Item.Type == ItemObject.ItemTypeEnum.Thrown)
                        hasThrowables = true;
                    if (item.Item.Type == ItemObject.ItemTypeEnum.Crossbow || item.Item.Type == ItemObject.ItemTypeEnum.Bow)
                        hasRanged = true;
                }
            }
            var sumOfAllArmor = troop.GetArmArmorSum() + troop.GetBodyArmorSum() + troop.GetHeadArmorSum() + troop.GetLegArmorSum();
            var horseArmor = troop.GetHorseArmorSum();

            var type = TroopType.ShockInfantry;

            if (hasRanged && !troop.IsMounted)
                return TroopType.Ranged;

            if (hasRanged && troop.IsMounted)
                return TroopType.HorseArcher;

            if (!hasRanged && !troop.IsMounted && hasThrowables && !hasShield && sumOfAllArmor < 75)
                return TroopType.SkirmishInfantry;

            if (!hasRanged && !troop.IsMounted && !hasShield)
                return TroopType.ShockInfantry;

            if(!hasRanged && !troop.IsMounted && hasShield)
                return TroopType.HeavyInfantry;

            if (!hasRanged && troop.IsMounted && (sumOfAllArmor > 75 || horseArmor > 40))
                return TroopType.HeavyCavalry;

            if (!hasRanged && troop.IsMounted && (sumOfAllArmor < 75 || horseArmor < 40))
                return TroopType.LightCavalry;

            return type;
        }

        private void SelectBattlesTactics()
        {
            if (!HasLeader)
            {
                var item = troop.Equipment[(EquipmentIndex)i];
                if (!item.IsEmpty)
                {
                    if (item.Item.Type == ItemObject.ItemTypeEnum.Shield)
                        hasShield = true;
                    if (item.Item.Type == ItemObject.ItemTypeEnum.Thrown)
                        hasThrowables = true;
                    if (item.Item.Type == ItemObject.ItemTypeEnum.Crossbow || item.Item.Type == ItemObject.ItemTypeEnum.Bow)
                        hasRanged = true;
                }
            }
            var sumOfAllArmor = troop.GetArmArmorSum() + troop.GetBodyArmorSum() + troop.GetHeadArmorSum() + troop.GetLegArmorSum();
            var horseArmor = troop.GetHorseArmorSum();

            var type = TroopType.ShockInfantry;

            if (hasRanged && !troop.IsMounted)
                return TroopType.Ranged;

            if (hasRanged && troop.IsMounted)
                return TroopType.HorseArcher;

            if (!hasRanged && !troop.IsMounted && hasThrowables && !hasShield && sumOfAllArmor < 75)
                return TroopType.SkirmishInfantry;

            if (!hasRanged && !troop.IsMounted && !hasShield)
                return TroopType.ShockInfantry;

            if(!hasRanged && !troop.IsMounted && hasShield)
                return TroopType.HeavyInfantry;

            if (!hasRanged && troop.IsMounted && (sumOfAllArmor > 75 || horseArmor > 40))
                return TroopType.HeavyCavalry;

            if (!hasRanged && troop.IsMounted && (sumOfAllArmor < 75 || horseArmor < 40))
                return TroopType.LightCavalry;

            return type;
        }

        private void SelectBattlesTactics()
        {
            if (!HasLeader)
            {
                return;
            }

            RollNewInfantryTactic();
            RollNewArchersTactic();
            RollNewCavalryTactic();
            RollNewHorseArchersTactic();
        }

        private void RollNewInfantryTactic()
        {
            CurrentInfantryTactic = (InfantryTactics)SubModule.Random.Next(1, EnumExtensions.GetEnumCount<InfantryTactics>() - 1);
        }

        private void RollNewArchersTactic()
        {
            CurrentArchersTactic = (RangedTactics)SubModule.Random.Next(1, EnumExtensions.GetEnumCount<RangedTactics>() - 1);
        }

        private void RollNewCavalryTactic()
        {
            CurrentCavalryTactic = (CavalryTactics)SubModule.Random.Next(1, EnumExtensions.GetEnumCount<CavalryTactics>() - 1);
        }

        private void RollNewHorseArchersTactic()
        {
            CurrentHorseArchersTactic = (HorseArchersTactics)SubModule.Random.Next(1, EnumExtensions.GetEnumCount<HorseArchersTactics>() - 1);
        }
    }
}
