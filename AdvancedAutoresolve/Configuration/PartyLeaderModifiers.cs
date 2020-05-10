using AdvancedAutoResolve.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAutoResolve.Configuration
{
    internal class PartyLeaderModifiers
    {
        [JsonProperty]
        internal Modifiers TacticsModifiers { get; set; }

        [JsonProperty]
        internal Modifiers LeadershipModifiers { get; set; }
    }
}
