using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using static TaleWorlds.CampaignSystem.MapEvent;

namespace AdvancedAutoResolve.Simulation.Models
{
    internal struct SimulationModel
    {
        internal MBGUID BattleId { get; }

        internal ReadOnlyCollection<Party> Parties { get; }

        internal string EventDescription { get; }

        internal bool IsPlayerInvolved { get; private set; }

        internal SimulationModel(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
        {
            BattleId = mapEvent.Id;
            EventDescription = mapEvent.ToString();
            Parties = new ReadOnlyCollection<Party>(new List<Party> { new Party(defenderParty), new Party(attackerParty) }); // do not change the order of the list
            IsPlayerInvolved = Hero.MainHero.Id == attackerParty.LeaderHero?.Id || Hero.MainHero.Id == defenderParty.LeaderHero?.Id;
        }

        internal int SimulateHit(MBGUID strikerTroopId, MBGUID strikedTroopId, float strikerAdvantage)
        {

            var attacker = GetTroop(strikerTroopId);
            var defender = GetTroop(strikedTroopId);

            //var attackersGrouped = attacker.PartyModel.Troops.GroupBy(t => t.TestTroopType).ToList();
            //var defendersGrouped = defender.PartyModel.Troops.GroupBy(t => t.TestTroopType).ToList();

            var troopNumbersAdvantage = CalculateNumbersAdvantage(attacker.PartyModel.Troops.Count, defender.PartyModel.Troops.Count);

            var attackerPower = attacker.GetPower();
            var attackerTacticModifiers = attacker.GetModifiersFromTactics();
            var attackerExtraPowerFromLeaderPerks = attacker.GetExtraAttackingPowerFromLeaderPerks(defender);
            var attackerLeaderAttackModifier = attacker.GetAttackModifierFromLeader();

            var defenderPower = defender.GetPower();
            var defenderTacticModifiers = defender.GetModifiersFromTactics();
            var defenderLeaderDefenseModifier = defender.GetDefenseModifierFromLeader();

            bool makesSenseToAttackThisUnit = attacker.DoesItMakeSenseToAttackThisUnit(defender);
            //if it doesn't make sense to attack current defender, reduce damage by 90%
            var makesSenseToAttackUnitModifier = makesSenseToAttackThisUnit ? 1f : 0.1f;

            var finalAttackerPower = attackerPower * attackerTacticModifiers.AttackBonus * attackerExtraPowerFromLeaderPerks * attackerLeaderAttackModifier * makesSenseToAttackUnitModifier;
            var finalDefenderPower = defenderPower * defenderTacticModifiers.DefenseBonus * defenderLeaderDefenseModifier;

            // 50f is from original calculation. Changed to 25, since new values are much higher
            var damage = (int)(25f * (finalAttackerPower / finalDefenderPower) * strikerAdvantage * troopNumbersAdvantage);

            return damage;
        }

        /// <summary>
        /// from -60% to 50% advantage from being outnumbered, or outnumbering the defender to be added to final damage.
        /// </summary>
        private float CalculateNumbersAdvantage(int attackersCount, int defendersCount)
        {
            var advantage = (float)((float)attackersCount / (float)defendersCount);
            if (advantage > 1.2f) return 1.2f;
            if (advantage < 0.8f) return 0.8f;
            return advantage;
        }

        internal void AddTroopsFromInvolvedParty(PartyBase involvedParty, BattleSideEnum side)
        {
            if (!IsPlayerInvolved && Hero.MainHero.Id == involvedParty.LeaderHero?.Id)
                IsPlayerInvolved = true;

            Parties[(int)side].AddTroopsFromParty(involvedParty);
        }

        internal void RemoveTroop(BattleSideEnum side, MBGUID troopId)
        {
            Parties[(int)side].RemoveTroop(troopId);
        }

        private Troop GetTroop(MBGUID troopId)
        {
            foreach (var party in Parties)
            {
                if (party.Troops.Any(it => it.CharacterObject.Id == troopId))
                {
                    return party.Troops.First(it => it.CharacterObject.Id == troopId);
                }
            }

            throw new Exception($"Could not find troop with Id {troopId}");
        }

        internal static bool IsValidEventType(BattleTypes battleType)
        {
            switch (battleType)
            {
                // normal battle
                case BattleTypes.FieldBattle:
                // village battles
                case BattleTypes.Raid:
                case BattleTypes.IsForcingSupplies:
                case BattleTypes.IsForcingVolunteers:
                // castle/town battles
                case BattleTypes.Siege:
                case BattleTypes.SiegeOutside:
                case BattleTypes.SallyOut:
                    return true;
                default:
                    return false;
            }
        }
    }
}
