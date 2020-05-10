using AdvancedAutoResolve.Configuration;
using AdvancedAutoResolve.Simulation;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AdvancedAutoResolve.Models
{
    internal class Troop
    {
        internal Troop(CharacterObject characterObject, Party partyModel, TroopType troopType)
        {
            CharacterObject = characterObject;
            Health = CharacterObject.MaxHitPoints();
            PartyModel = partyModel;
            TroopType = troopType;
        }

        internal CharacterObject CharacterObject { get; }
        internal int Health { get; set; }
        internal Party PartyModel { get; }
        internal TroopType TroopType { get; }

        /// <summary>
        /// Subtracts <paramref name="damage"/> amount from <see cref="Troop.Health"/>
        /// </summary>
        /// <param name="damage">damage amount</param>
        /// <returns><c>true</c> if <see cref="Troop.Health"/> is 0 or below</returns>
        internal bool ApplyDamage(int damage)
        {
            Health -= damage;
            return Health <= 0;
        }

        internal Modifiers GetModifiersFromTactics()
        {
            switch (TroopType)
            {
                case TroopType.ShockInfantry:
                case TroopType.SkirmishInfantry:
                case TroopType.HeavyInfantry:
                    return PartyModel.CurrentInfantryTactic.Modifiers;
                case TroopType.Ranged:
                    return PartyModel.CurrentRangedTactic.Modifiers;
                case TroopType.LightCavalry:
                case TroopType.HeavyCavalry:
                    return PartyModel.CurrentCavalryTactic.Modifiers;
                case TroopType.HorseArcher:
                    return PartyModel.CurrentHorseArchersTactic.Modifiers;
                default:
                    throw new NotImplementedException($"Not supported TroopType {TroopType}");
            }
        }

        /// <summary>
        /// Vanilla troop power plus 5% with each troop tier (up to 30% at T6)
        /// </summary>
        internal float GetPower()
        {
            var basePower = CharacterObject.GetPower();
            var tier = (float)CharacterObject.Tier;
            var finalPower = basePower + tier / 20f; // 5% extra power per unit tier
            return finalPower;
        }

        /// <summary>
        /// Decide whether the defender would be attacked at this point in time, or not.
        /// </summary>
        internal bool DoesItMakeSenseToAttackThisUnit(Troop defender)
        {
            if (TroopType == TroopType.HeavyInfantry)
            {
                if (defender.TroopType == TroopType.Ranged
                    && defender.PartyModel.CurrentRangedTactic == Config.CurrentConfig.Tactics.Find(t => t.Name == "SkirmishBehindInfantry")
                    && defender.PartyModel.HasInfantry)
                {
                    // attacker is infantry, and the defender is an archer in skirmish tactic and his party still has infantry to cower behind.
                    return false;
                }
            }
            return true;
        }

        internal float GetDefenseModifierFromLeader()
        {
            float modifier = 1f;

            if (PartyModel.HasLeader)
            {
                modifier += PartyModel.PartyLeader.TacticsLevel * (Config.CurrentConfig.PartyLeaderModifiers.TacticsModifiers.DefenseBonus - 1f) / 100;
                modifier += PartyModel.PartyLeader.LeadershipLevel * (Config.CurrentConfig.PartyLeaderModifiers.LeadershipModifiers.DefenseBonus - 1f) / 100;
            }

            return modifier;
        }

        internal float GetAttackModifierFromLeader()
        {
            float modifier = 1f;

            if (PartyModel.HasLeader)
            {
                modifier += PartyModel.PartyLeader.TacticsLevel * (Config.CurrentConfig.PartyLeaderModifiers.TacticsModifiers.AttackBonus - 1f) / 100;
                modifier += PartyModel.PartyLeader.LeadershipLevel * (Config.CurrentConfig.PartyLeaderModifiers.LeadershipModifiers.AttackBonus -1f) / 100;
            }

            return modifier;
        }

        internal Modifiers GetSiegeDefenderModifiers()
        {
            return PartyModel.IsSiegeDefender ? Config.CurrentConfig.SiegeDefendersModifiers : Modifiers.GetDefaultModifiers();
        }

        internal float GetExtraAttackingPowerFromLeaderPerks(Troop defender)
        {
            float modifier = 1f;
            if (PartyModel.HasLeader)
            {
                if (PartyModel.PartyLeader.HasTacticalSuperiorityPerk)
                {
                    modifier += 0.05f;
                }
                if (PartyModel.PartyLeader.HasHammerAndAnvilPerk)
                {
                    //TODO Can HA use this perk as well??
                    if (TroopType == TroopType.LightCavalry && defender.TroopType == TroopType.Ranged)
                    {
                        modifier += 0.5f;
                    }
                }
                if (PartyModel.PartyLeader.HasPhalanxPerk)
                {
                    if (TroopType == TroopType.HeavyInfantry && defender.TroopType == TroopType.LightCavalry || defender.TroopType == TroopType.HorseArcher)
                    {
                        modifier += 0.5f;
                    }
                }
                if (PartyModel.PartyLeader.HasAmbushSpecialistPerk && PartyModel.TerrainType == TerrainType.Forest)
                {
                    if (TroopType == TroopType.Ranged)
                    {
                        modifier += 0.6f;
                    }
                }
            }

            return modifier;
        }

        public override string ToString()
        {
            return CharacterObject.ToString();
        }
    }
}
