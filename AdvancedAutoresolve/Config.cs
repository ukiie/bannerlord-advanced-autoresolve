using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAutoResolve
{
    internal class Config
    {
        internal static Config CurrentConfig { get; private set; }

        [JsonProperty]
        internal bool DetailedLogging { get; set; }
        [JsonProperty]
        internal bool OnlyPlayerBattles { get; set; }

        internal bool ShouldLogThis(bool isPlayerInvolved = true) => DetailedLogging && (OnlyPlayerBattles && isPlayerInvolved || !OnlyPlayerBattles);

        private static readonly string ConfigFilePath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");

        private static readonly bool configExists = File.Exists(ConfigFilePath);

        internal static void InitializeConfig()
        {
            var config = new Config()
            {
                DetailedLogging = true,
                OnlyPlayerBattles = true
            };

            try
            {
                if (configExists)
                {
                    config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigFilePath));
                }
            }
            finally
            {
                CurrentConfig = config;
            }
        }
    }
}
