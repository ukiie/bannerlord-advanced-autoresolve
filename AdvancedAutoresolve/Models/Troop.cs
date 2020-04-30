﻿using AdvancedAutoResolve.Simulation;
using System;
using TaleWorlds.CampaignSystem;

namespace AdvancedAutoResolve.Models
{
    internal class Troop
    {
        internal Troop(CharacterObject characterObject, Party partyModel, TroopType troopType)
        {
            CharacterObject = characterObject;
            PartyModel = partyModel;
            TroopType = troopType;
        }

        internal CharacterObject CharacterObject { get; }
        internal Party PartyModel { get; }
        internal TroopType TroopType { get; }

        internal Modifiers GetModifiersFromTactics()
        {
            switch (TroopType)
            {
                case TroopType.Infantry:
                    return TacticsModifiers.GetModifiersFromInfantryTactic(PartyModel.CurrentInfantryTactic);
                case TroopType.Archer:
                    return TacticsModifiers.GetModifiersFromArcherTactic(PartyModel.CurrentArchersTactic);
                case TroopType.Cavalry:
                    return TacticsModifiers.GetModifiersFromCavalryTactic(PartyModel.CurrentCavalryTactic);
                case TroopType.HorseArcher:
                    return TacticsModifiers.GetModifiersFromHorseArcherTactic(PartyModel.CurrentHorseArchersTactic);
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
        /// 5% bonus per 100 levels of a skill
        /// </summary>
        internal float GetAttackModifierFromLeader()
        {
            float modifier = 1f;

            if (PartyModel.HasLeader)
            {
                modifier += PartyModel.PartyLeader.TacticsLevel / 2000;
                modifier += PartyModel.PartyLeader.LeadershipLevel / 2000;
            }

            return modifier;
        }

        /// <summary>
        /// Decide whether the defender would be attacked at this point in time, or not.
        /// </summary>
        internal bool DoesItMakeSenseToAttackThisUnit(Troop defender)
        {
            if (TroopType == TroopType.Infantry)
            {
                if (defender.TroopType == TroopType.Archer && defender.PartyModel.CurrentArchersTactic == ArchersTactics.Skirmish && defender.PartyModel.HasInfantry)
                {
                    // attacker is infantry, and the defender is an archer in skirmish tactic and his party still has infantry to cower behind.
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 5% bonus per 100 levels of a skill
        /// </summary>
        internal float GetDefenseModifierFromLeader()
        {
            float modifier = 1f;

            if (PartyModel.HasLeader)
            {
                modifier += PartyModel.PartyLeader.TacticsLevel / 2000;
                modifier += PartyModel.PartyLeader.LeadershipLevel / 2000;
            }

            return modifier;
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
                    if (TroopType == TroopType.Cavalry && defender.TroopType == TroopType.Archer)
                    {
                        modifier += 0.5f;
                    }
                }
                if (PartyModel.PartyLeader.HasPhalanxPerk)
                {
                    if (TroopType == TroopType.Infantry && defender.TroopType == TroopType.Cavalry || defender.TroopType == TroopType.HorseArcher)
                    {
                        modifier += 0.5f;
                    }
                }
            }

            return modifier;
        }
    }
}