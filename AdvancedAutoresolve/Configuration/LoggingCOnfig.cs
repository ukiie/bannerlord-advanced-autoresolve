using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAutoResolve.Configuration
{
    internal class LoggingConfig
    {
        [JsonProperty]
        internal bool DetailedLogging { get; set; }

        [JsonProperty]
        internal bool LogOnlyPlayerBattles { get; set; }

        public LoggingConfig()
        {

        }

        public LoggingConfig(bool detailedLogging, bool logOnlyPlayerBattles)
        {
            DetailedLogging = detailedLogging;
            LogOnlyPlayerBattles = logOnlyPlayerBattles;
        }
    }
}
