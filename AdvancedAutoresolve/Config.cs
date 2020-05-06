using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAutoResolve
{
    internal class Config
    {
        internal static Config CurrentConfig;

        internal bool DetailedLogging { get; set; }
        internal bool OnlyPlayerBattles { get; set; }

        internal bool ShouldLogThis(bool isPlayerInvolved = true) => DetailedLogging && (OnlyPlayerBattles && isPlayerInvolved || !OnlyPlayerBattles);

        internal static void InitializeConfig()
        {
            CurrentConfig = new Config()
            {
                DetailedLogging = true,
                OnlyPlayerBattles = true
            };
        }
    }
}
