using AdvancedAutoResolve.Configuration;
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
using TaleWorlds.ObjectSystem;
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

            bool isSiegeBattle = mapEvent.EventType == BattleTypes.Siege;

            Parties = new ReadOnlyCollection<Party>(new List<Party> { new Party(defenderParty, isSiegeBattle), new Party(attackerParty) }); // do not change the order of the list
            IsPlayerInvolved = Hero.MainHero.Id == attackerParty.LeaderHero?.Id || Hero.MainHero.Id == defenderParty.LeaderHero?.Id;
        }

        internal int SimulateHit(MBGUID strikerTroopId, MBGUID strikedTroopId, float strikerAdvantage)
        {
            var attacker = GetTroop(strikerTroopId);
            var defender = GetTroop(strikedTroopId);

            var troopNumbersAdvantage = CalculateNumbersAdvantage(attacker.PartyModel.Troops.Count, defender.PartyModel.Troops.Count);

            var attackerPower = attacker.GetPower();
            var attackerTacticModifiers = attacker.GetModifiersFromTactics();
            var attackerSiegeModifiers = attacker.GetSiegeDefenderModifiers();
            var attackerExtraPowerFromLeaderPerks = attacker.GetExtraAttackingPowerFromLeaderPerks(defender);
            var attackerLeaderAttackModifier = attacker.GetAttackModifierFromLeader();

            var defenderPower = defender.GetPower();
            var defenderTacticModifiers = defender.GetModifiersFromTactics();
            var defenderSiegeModifiers = defender.GetSiegeDefenderModifiers();
            var defenderLeaderDefenseModifier = defender.GetDefenseModifierFromLeader();

            bool makesSenseToAttackThisUnit = attacker.DoesItMakeSenseToAttackThisUnit(defender);
            //if it doesn't make sense to attack current defender, reduce damage by the value from config
            var makesSenseToAttackUnitModifier = makesSenseToAttackThisUnit ? 1f : 1f + (float)Config.CurrentConfig.DoesntMakeSenseToAttackModifier / 100;

            var finalAttackerPower = attackerPower 
                * attackerTacticModifiers.AttackBonus 
                * attackerSiegeModifiers.AttackBonus 
                * attackerExtraPowerFromLeaderPerks 
                * attackerLeaderAttackModifier 
                * makesSenseToAttackUnitModifier;

            var finalDefenderPower = defenderPower 
                * defenderTacticModifiers.DefenseBonus 
                * defenderSiegeModifiers.DefenseBonus 
                * defenderLeaderDefenseModifier;

            var damage = (int)(50f * (finalAttackerPower / finalDefenderPower) * strikerAdvantage * troopNumbersAdvantage);

            return damage;
        }

        private float CalculateNumbersAdvantage(int attackersCount, int defendersCount)
        {
            float advantage = (float)((float)attackersCount / (float)defendersCount);

            float highCap = (float)Config.CurrentConfig.NumbersAdvantageModifier.HighCap / 100;
            float lowCap = (float)Config.CurrentConfig.NumbersAdvantageModifier.LowCap / 100;

            if (advantage > highCap) return highCap;
            if (advantage < lowCap) return lowCap;

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
            return Config.CurrentConfig.ValidBattleTypes.Contains(battleType.ToString());
        }
    }
}
