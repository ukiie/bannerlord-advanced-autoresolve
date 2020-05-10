using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAutoResolve.Models
{
    internal class Tactic
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public Modifiers Modifiers { get; set; }

        [JsonProperty]
        public List<TroopType> TroopTypes { get; set; }

        public Tactic()
        {

        }

        public Tactic(string name, Modifiers modifiers, List<TroopType> troopTypes)
        {
            Name = name;
            Modifiers = modifiers;
            TroopTypes = troopTypes;
        }
    }
}
