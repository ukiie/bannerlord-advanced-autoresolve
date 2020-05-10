namespace AdvancedAutoResolve.Models
{
    internal enum TroopType
    {
        Any = 0,
        ShockInfantry = 1,
        SkirmishInfantry = 2,
        HeavyInfantry = 3,
        Ranged = 4,
        LightCavalry = 5,
        HeavyCavalry = 6,
        HorseArcher = 7
    }

    internal enum SimulationTroopState
    {
        Alive = 0,
        Wounded = 1,
        Kille = 2,
        Routed = 3
    }

    internal enum InfantryTactics
    {
        NoTactic = 0,
        Charge = 1,
        Advance = 2,
        ShieldWall = 3,
    }

    internal enum RangedTactics
    {
        NoTactic = 0,
        Charge = 1,
        SkirmishBehindInfantry = 2,
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
