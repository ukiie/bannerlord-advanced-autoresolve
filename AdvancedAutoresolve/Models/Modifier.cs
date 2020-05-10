using Newtonsoft.Json;

namespace AdvancedAutoResolve.Models
{
    internal struct Modifiers
    {
        internal Modifiers(float attackBonus, float defenseBonus)
        {
            AttackBonus = attackBonus;
            DefenseBonus = defenseBonus;
        }
        
        [JsonConstructor]
        internal Modifiers(int attackBonus, int defenseBonus)
        {
            AttackBonus = 1f + (float)attackBonus / 100;
            DefenseBonus = 1f + (float)defenseBonus / 100;
        }

        [JsonProperty]
        internal float AttackBonus { get; }
        [JsonProperty]
        internal float DefenseBonus { get; }

        internal static Modifiers GetNoTacticModifiers()
        {
            return new Modifiers(0.75f, 0.75f);
        }

        internal static Modifiers GetDefaultModifiers()
        {
            return new Modifiers(1f, 1f);
        }

    }
}
