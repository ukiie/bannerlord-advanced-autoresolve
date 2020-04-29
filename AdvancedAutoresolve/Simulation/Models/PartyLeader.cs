using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAutoResolve.Simulation.Models
{
    internal struct PartyLeader
    {
        public PartyLeader(int leadershipLevel, int tacticsLevel, bool hasTacticalSuperiorityPerk, bool hasHammerAndAnvilPerk, bool hasPhalanxPerk)
        {
            LeadershipLevel = leadershipLevel;
            TacticsLevel = tacticsLevel;
            HasTacticalSuperiorityPerk = hasTacticalSuperiorityPerk;
            HasHammerAndAnvilPerk = hasHammerAndAnvilPerk;
            HasPhalanxPerk = hasPhalanxPerk;
        }

        public int LeadershipLevel { get; }
        public int TacticsLevel { get; }
        public bool HasTacticalSuperiorityPerk { get; }
        public bool HasHammerAndAnvilPerk { get; }
        public bool HasPhalanxPerk { get; }
    }
}
