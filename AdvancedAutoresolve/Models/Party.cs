using AdvancedAutoResolve.Configuration;
using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Simulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace AdvancedAutoResolve.Models
{
    internal class Party
    {
        internal bool HasLeader { get; }

        internal bool IsSiegeDefender { get; }

        internal TerrainType TerrainType { get; }

        //TODO Change this or add separately all Heros from parties to give exta bonuses to troops from perks
        internal PartyLeader PartyLeader { get; }
        internal Tactic CurrentInfantryTactic { get; private set; }
        internal Tactic CurrentRangedTactic { get; private set; }
        internal Tactic CurrentCavalryTactic { get; private set; }
        internal Tactic CurrentHorseArchersTactic { get; private set; }

        internal List<Troop> Troops { get; }

        internal bool HasInfantry => Troops.Any(t => TroopTypeExtensions.IsInfantryTroopType((int)t.TroopType));
        internal bool HasArchers => Troops.Any(t => TroopTypeExtensions.IsRangedTroopType((int)t.TroopType));
        internal bool HasCavalry => Troops.Any(t => TroopTypeExtensions.IsCavalryTroopType((int)t.TroopType));
        internal bool HasHorseArchers => Troops.Any(t => TroopTypeExtensions.IsHorseArcherTroopType((int)t.TroopType));


        internal Party(PartyBase party, bool isSiegeDefender = false)
        {
            TerrainType = Campaign.Current.MapSceneWrapper.GetTerrainTypeAtPosition(party.MapEvent.Position);
            IsSiegeDefender = isSiegeDefender;
            
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
                var troopType = troop.Character.DecideTroopType();

                while (totalNumber-- > 0)
                {
                    Troops.Add(new Troop(troop.Character, this, troopType));
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

        private void SelectBattlesTactics()
        {
            if (!HasLeader)
            {
                SetNoTacticsToAllFormations();
                return;
            }

            RollNewInfantryTactic();
            RollNewArchersTactic();
            RollNewCavalryTactic();
            RollNewHorseArchersTactic();
        }

        private void SetNoTacticsToAllFormations()
        {
            var tactic = Config.CurrentConfig.Tactics.Find(t => t.TroopTypes.Any(tt=>tt == TroopType.Any));
            CurrentInfantryTactic = tactic;
            CurrentRangedTactic = tactic;
            CurrentCavalryTactic = tactic;
            CurrentHorseArchersTactic = tactic;
        }

        private void RollNewInfantryTactic()
        {
            var validTactics = Config.CurrentConfig.Tactics.FindAll(t => t.TroopTypes.Any(tt => ((int)tt).IsInfantryTroopType()));
            CurrentInfantryTactic = validTactics[SubModule.Random.Next(0, validTactics.Count)];
        }

        private void RollNewArchersTactic()
        {
            var validTactics = Config.CurrentConfig.Tactics.FindAll(t => t.TroopTypes.Any(tt => ((int)tt).IsRangedTroopType()));
            CurrentRangedTactic = validTactics[SubModule.Random.Next(0, validTactics.Count)];
        }

        private void RollNewCavalryTactic()
        {
            var validTactics = Config.CurrentConfig.Tactics.FindAll(t => t.TroopTypes.Any(tt => ((int)tt).IsCavalryTroopType()));
            CurrentCavalryTactic = validTactics[SubModule.Random.Next(0, validTactics.Count)];
        }

        private void RollNewHorseArchersTactic()
        {
            var validTactics = Config.CurrentConfig.Tactics.FindAll(t => t.TroopTypes.Any(tt => ((int)tt).IsHorseArcherTroopType()));
            CurrentHorseArchersTactic = validTactics[SubModule.Random.Next(0, validTactics.Count)];
        }
    }
}
