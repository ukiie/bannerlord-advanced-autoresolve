namespace AdvancedAutoResolve.Models
{
    internal struct PartyLeader
    {
        internal PartyLeader(int leadershipLevel, int tacticsLevel, bool hasTacticalSuperiorityPerk, bool hasHammerAndAnvilPerk, bool hasPhalanxPerk, bool hasAmbushSpecialistPerk)
        {
            LeadershipLevel = leadershipLevel;
            TacticsLevel = tacticsLevel;
            HasTacticalSuperiorityPerk = hasTacticalSuperiorityPerk;
            HasHammerAndAnvilPerk = hasHammerAndAnvilPerk;
            HasPhalanxPerk = hasPhalanxPerk;
            HasAmbushSpecialistPerk = hasAmbushSpecialistPerk;
        }

        internal int LeadershipLevel { get; }
        internal int TacticsLevel { get; }
        internal bool HasTacticalSuperiorityPerk { get; }
        internal bool HasHammerAndAnvilPerk { get; }
        internal bool HasPhalanxPerk { get; }
        internal bool HasAmbushSpecialistPerk { get; }
    }
}
