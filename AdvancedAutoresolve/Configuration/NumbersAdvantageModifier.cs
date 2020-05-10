using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAutoResolve.Configuration
{
    internal class NumbersAdvantageModifier
    {
        [JsonProperty]
        internal int HighCap { get; set; }
        [JsonProperty]
        internal int LowCap { get; set; }
    }
}
