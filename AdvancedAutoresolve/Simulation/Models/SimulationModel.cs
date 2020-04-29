using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace AdvancedAutoResolve.Simulation.Models
{
    internal struct SimulationModel
    {
        internal MBGUID BattleId { get; }

        private ReadOnlyCollection<PartyModel> Parties { get; }

        internal string EventDescription { get; }

        internal SimulationModel(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
        {
            BattleId = mapEvent.Id;
            EventDescription = mapEvent.ToString();
            Parties = new ReadOnlyCollection<PartyModel>(new List<PartyModel> { new PartyModel(defenderParty), new PartyModel(attackerParty) }); // do not change the order of the list
        }

        internal int SimulateHit(MBGUID strikerTroopId, MBGUID strikedTroopId, float strikerAdvantage)
        {
            var attacker = GetTroop(strikerTroopId);
            var defender = GetTroop(strikedTroopId);

            var attackerPower = attacker.CharacterObject.GetPower();
            var attackerTacticModifiers = attacker.GetModifiersFromTactics();
            var attackerExtraPowerFromLeaderPerks = attacker.GetExtraAttackingPowerFromLeaderPerks(defender);
            var attackerLeaderAttackModifier = attacker.GetAttackModifierFromLeader();

            var defenderPower = defender.CharacterObject.GetPower();
            var defenderTacticModifiers = defender.GetModifiersFromTactics();
            var defenderLeaderDefenseModifier = defender.GetDefenseModifierFromLeader();

            var finalAttackerPower = attackerPower * attackerTacticModifiers.AttackBonus * attackerExtraPowerFromLeaderPerks * attackerLeaderAttackModifier;
            var finalDefenderPower = defenderPower * defenderTacticModifiers.DefenseBonus * defenderLeaderDefenseModifier;

            var damage = (int)(50f * (finalAttackerPower / finalDefenderPower) * strikerAdvantage);

            return damage;
        }

        internal void AddTroopsFromInvolvedParty(PartyBase involvedParty, BattleSideEnum side)
        {
            Parties[(int)side].AddTroopsFromParty(involvedParty);
        }

        internal void RemoveTroop(BattleSideEnum side, MBGUID troopId)
        {
            Parties[(int)side].RemoveTroop(troopId);
        }

        internal void ChangeTactics(BattleSideEnum side)
        {
            Parties[(int)side].SelectNewTactics();
        }

        private TroopModel GetTroop(MBGUID troopId)
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
    }
}
