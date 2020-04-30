namespace AdvancedAutoResolve.Models
{
    internal enum TroopType
    {
        Infantry = 1,
        Archer = 2,
        Cavalry = 3,
        HorseArcher = 4
    }

    internal enum SimulationTroopState
    {
        Alive,
        Wounded,
        Killed,
        Routed
    }

    internal enum InfantryTactics
    {
        NoTactic = 0,
        Charge = 1,
        Advance = 2,
        ShieldWall = 3,
    }

    internal enum ArchersTactics
    {
        NoTactic = 0,
        Charge = 1,
        Skirmish = 2,
    }

    internal enum CavalryTactics
    {
        NoTactic = 0,
        Charge = 1,
        Flank = 2,
    }

    internal enum HorseArchersTactics
    {
        NoTactic = 0,
        Charge = 1,
        Skirmish = 2,
    }

}
