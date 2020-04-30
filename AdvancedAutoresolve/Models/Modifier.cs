namespace AdvancedAutoResolve.Models
{
    internal struct Modifiers
    {
        internal Modifiers(float attackBonus, float defenseBonus)
        {
            AttackBonus = attackBonus;
            DefenseBonus = defenseBonus;
        }

        internal float AttackBonus { get; }
        internal float DefenseBonus { get; }

        internal static Modifiers GetDefaultModifiers()
        {
            return new Modifiers(0.7f, 0.7f);
        }

    }
}
