using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace AdvancedAutoResolve.Simulation.Models
{
    internal class TroopModel
    {
        internal TroopModel(CharacterObject characterObject, PartyModel partyModel, TroopType troopType)
        {
            CharacterObject = characterObject;
            PartyModel = partyModel;
            TroopType = troopType;
        }

        internal CharacterObject CharacterObject { get; }
        internal PartyModel PartyModel { get; }
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

        internal float GetExtraAttackingPowerFromLeaderPerks(TroopModel defender)
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
