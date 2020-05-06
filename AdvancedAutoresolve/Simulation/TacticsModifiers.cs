using AdvancedAutoResolve.Models;
using AdvancedAutoResolve.Simulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAutoResolve.Simulation
{
    internal static class TacticsModifiers
    {
        internal static Modifiers GetModifiersFromInfantryTactic(InfantryTactics tactic)
        {
            switch (tactic)
            {
                case InfantryTactics.Advance:
                    return new Modifiers(1.1f, 1.2f);
                case InfantryTactics.Charge:
                    return new Modifiers(1.2f, 0.9f);
                case InfantryTactics.ShieldWall:
                    return new Modifiers(0.9f, 1.5f);
                default:
                    return Modifiers.GetDefaultModifiers();
            }
        }

        internal static Modifiers GetModifiersFromArcherTactic(RangedTactics tactic)
        {
            switch (tactic)
            {
                case RangedTactics.SkirmishBehindInfantry:
                    return new Modifiers(1.2f, 1.4f);
                default:
                    return Modifiers.GetDefaultModifiers();
            }
        }

        internal static Modifiers GetModifiersFromCavalryTactic(CavalryTactics tactic)
        {
            switch (tactic)
            {
                case CavalryTactics.Charge:
                    return new Modifiers(1.3f, 0.9f);
                case CavalryTactics.Flank:
                    return new Modifiers(1.1f, 1.3f);
                default:
                    return Modifiers.GetDefaultModifiers();
            }
        }

        internal static Modifiers GetModifiersFromHorseArcherTactic(HorseArchersTactics tactic)
        {
            switch (tactic)
            {
                case HorseArchersTactics.Skirmish:
                    return new Modifiers(1.1f, 1.5f);
                default:
                    return Modifiers.GetDefaultModifiers();
            }
        }
    }
}
