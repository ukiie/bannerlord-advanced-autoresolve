using AdvancedAutoResolve.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace AdvancedAutoResolve.Simulation.Models
{
    internal class PartyModel
    {

        internal bool HasLeader { get; }

        //TODO Change this or add separately all Heros from parties to give exta bonuses to troops from perks
        internal PartyLeader PartyLeader { get; }
        internal InfantryTactics CurrentInfantryTactic { get; private set; } = InfantryTactics.NoTactic;
        internal ArchersTactics CurrentArchersTactic { get; private set; } = ArchersTactics.NoTactic;
        internal CavalryTactics CurrentCavalryTactic { get; private set; } = CavalryTactics.NoTactic;
        internal HorseArchersTactics CurrentHorseArchersTactic { get; private set; } = HorseArchersTactics.NoTactic;

        internal List<TroopModel> Troops { get; }

        private bool HasInfantry => Troops.Any(t => t.TroopType == TroopType.Infantry);
        private bool HasArchers => Troops.Any(t => t.TroopType == TroopType.Archer);
        private bool HasCavalry => Troops.Any(t => t.TroopType == TroopType.Cavalry);
        private bool HasHorseArchers => Troops.Any(t => t.TroopType == TroopType.HorseArcher);


        internal PartyModel(PartyBase party)
        {
            HasLeader = party.LeaderHero != null && party.LeaderHero.HitPoints > 20; // leader hero is present and not wounded
            if (HasLeader)
            {
                PartyLeader = new PartyLeader(
                    party.LeaderHero.GetSkillValue(DefaultSkills.Leadership),
                    party.LeaderHero.GetSkillValue(DefaultSkills.Tactics),
                    party.LeaderHero.GetPerkValue(DefaultPerks.Tactics.TacticalSuperiority),
                    party.LeaderHero.GetPerkValue(DefaultPerks.Tactics.HammerAndAnvil),
                    party.LeaderHero.GetPerkValue(DefaultPerks.Tactics.Phalanx));
            }

            Troops = new List<TroopModel>();

            AddTroopsFromParty(party);

            SelectInitialTactics();
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

                while (totalNumber-- > 0)
                {
                    Troops.Add(new TroopModel(troop.Character, this, troopType));
                }
            }
            if(party.MobileParty.AttachedParties != null)
            {
                foreach(var attachedParty in party.MobileParty.AttachedParties)
                {
                    AddTroopsFromParty(attachedParty.Party);
                }
            }
        }

        private TroopType DecideTroopType(CharacterObject troop)
        {
            if (troop.CurrentFormationClass == FormationClass.Infantry)
                return TroopType.Infantry;
            if (troop.CurrentFormationClass == FormationClass.Ranged)
                return TroopType.Archer;
            if (troop.CurrentFormationClass == FormationClass.Cavalry)
                return TroopType.Cavalry;
            if (troop.CurrentFormationClass == FormationClass.HorseArcher)
                return TroopType.HorseArcher;

            return TroopType.Infantry;
        }

        private int GetTotalNumberOfFormations()
        {
            int number = 0;
            if (HasInfantry)
                number++;
            if (HasArchers)
                number++;
            if (HasCavalry)
                number++;
            if (HasHorseArchers)
                number++;

            return number;
        }

        private void SelectInitialTactics()
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

        internal void SelectNewTactics()
        {
            if (!HasLeader)
            {
                return;
            }

            int totalNumberOfFormations = GetTotalNumberOfFormations();
            int numberOfFormationsThatWillSwitchTactics = SubModule.Random.Next(1, totalNumberOfFormations);

            bool changedInfTactic = false;
            bool changedArchTactic = false;
            bool changedCavTactic = false;
            bool changedHATactic = false;

            while (numberOfFormationsThatWillSwitchTactics > 0)
            {
                int formationIndex = SubModule.Random.Next(1, 4);

                if (!changedInfTactic && formationIndex == (int)TroopType.Infantry && HasInfantry)
                {
                    RollNewInfantryTactic();
                    changedInfTactic = true;
                    numberOfFormationsThatWillSwitchTactics--;
                }
                if (!changedArchTactic && formationIndex == (int)TroopType.Archer && HasArchers)
                {
                    RollNewArchersTactic();
                    changedArchTactic = true;
                    numberOfFormationsThatWillSwitchTactics--;
                }
                if (!changedCavTactic && formationIndex == (int)TroopType.Cavalry && HasCavalry)
                {
                    RollNewCavalryTactic();
                    changedCavTactic = true;
                    numberOfFormationsThatWillSwitchTactics--;
                }
                if (!changedHATactic && formationIndex == (int)TroopType.HorseArcher && HasHorseArchers)
                {
                    RollNewHorseArchersTactic();
                    changedHATactic = true;
                    numberOfFormationsThatWillSwitchTactics--;
                }
            }
        }

        private void RollNewInfantryTactic()
        {
            CurrentInfantryTactic = (InfantryTactics)SubModule.Random.Next(1, EnumExtensions.GetEnumCount<InfantryTactics>() - 1);
        }

        private void RollNewArchersTactic()
        {
            CurrentArchersTactic = (ArchersTactics)SubModule.Random.Next(1, EnumExtensions.GetEnumCount<ArchersTactics>() - 1);
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
