using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static TaleWorlds.CampaignSystem.MapEvent;

namespace AdvancedAutoResolve.Configuration
{
    internal class Config
    {
        internal static bool ConfigLoaded { get; set; }
        internal static string ConfigError { get; set; }
        internal static Config CurrentConfig { get; private set; }

        [JsonProperty]
        internal LoggingConfig Logging { get; set; }

        [JsonProperty]
        internal bool EnabledForAI { get; set; }

        [JsonProperty]
        internal List<Tactic> Tactics { get; set; }

        [JsonProperty]
        internal Modifiers SiegeDefendersModifiers { get; set; }

        [JsonProperty]
        internal PartyLeaderModifiers PartyLeaderModifiers { get; set; }

        [JsonProperty]
        internal NumbersAdvantageModifier NumbersAdvantageModifier { get; set; }

        [JsonProperty]
        internal List<string> ValidBattleTypes { get; set; }

        [JsonProperty]
        internal int DoesntMakeSenseToAttackModifier { get; set; }

        internal bool ShouldLogThis(bool isPlayerInvolved = true) => Logging.DetailedLogging && (Logging.LogOnlyPlayerBattles && isPlayerInvolved || !Logging.LogOnlyPlayerBattles);

        private static readonly string ConfigFilePath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");

        private static readonly bool configExists = File.Exists(ConfigFilePath);

        internal static void InitializeConfig()
        {
            var config = CreateDefaultConfig();

            try
            {
                if (configExists)
                {
                    config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigFilePath));
                    ConfigLoaded = true;
                }
                else
                {
                    throw new Exception($"AdvancedAutoResolve config not found at {ConfigFilePath}");
                }
            }
            catch (Exception ex)
            {
                ConfigError = "Failed to load AdvancedAutoresolve config. Using default values. Error: " + ex.Message;
            }
            finally
            {
                CurrentConfig = config;
            }
        }

        private static Config CreateDefaultConfig()
        {
            return new Config
            {
                Logging = new LoggingConfig()
                {
                    DetailedLogging = false,
                    LogOnlyPlayerBattles = true
                },
                EnabledForAI = true,
                ValidBattleTypes = new List<string> {
                    BattleTypes.FieldBattle.ToString(),
                    BattleTypes.Raid.ToString(),
                    BattleTypes.IsForcingSupplies.ToString(),
                    BattleTypes.IsForcingVolunteers.ToString(),
                    BattleTypes.Siege.ToString(),
                    BattleTypes.SiegeOutside.ToString(),
                    BattleTypes.SallyOut.ToString() 
                },
                Tactics = new List<Tactic>
                {
                    new Tactic("NoTactic", new Modifiers(0.75f, 0.75f), new List<TroopType>{ TroopType.Any }),

                    new Tactic("Charge", new Modifiers(1.2f, 0.9f), new List<TroopType>{ TroopType.ShockInfantry, TroopType.SkirmishInfantry, TroopType.HeavyInfantry }),
                    new Tactic("Advance", new Modifiers(1f, 1.2f), new List<TroopType>{ TroopType.ShockInfantry, TroopType.SkirmishInfantry, TroopType.HeavyInfantry }),
                    new Tactic("ShieldWall", new Modifiers(0.75f, 0.75f), new List<TroopType>{ TroopType.ShockInfantry, TroopType.SkirmishInfantry, TroopType.HeavyInfantry }),

                    new Tactic("SkirmishInFrontOfInfantry", new Modifiers(1.5f, 0.75f), new List<TroopType>{ TroopType.Ranged }),
                    new Tactic("SkirmishBehindInfantry", new Modifiers(0.75f, 1.5f), new List<TroopType>{ TroopType.Ranged }),
                    new Tactic("SkirmishFromAFlank", new Modifiers(1.25f, 1.25f), new List<TroopType>{ TroopType.Ranged }),

                    new Tactic("Charge", new Modifiers(1.5f, 0.85f), new List<TroopType>{ TroopType.LightCavalry, TroopType.HeavyCavalry }),
                    new Tactic("Flank", new Modifiers(1.25f, 1.1f), new List<TroopType>{ TroopType.LightCavalry, TroopType.HeavyCavalry }),
                    new Tactic("HitAndRun", new Modifiers(1.25f, 0.75f), new List<TroopType>{ TroopType.LightCavalry, TroopType.HeavyCavalry }),

                    new Tactic("Charge", new Modifiers(1.25f, 0.75f), new List<TroopType>{ TroopType.HorseArcher }),
                    new Tactic("Skirmish", new Modifiers(0.75f, 1.25f), new List<TroopType>{ TroopType.HorseArcher }),
                    new Tactic("HitAndRun", new Modifiers(1.5f, 0.5f), new List<TroopType>{ TroopType.HorseArcher }),
                },
                SiegeDefendersModifiers = new Modifiers(1.5f, 1.5f),
                PartyLeaderModifiers = new PartyLeaderModifiers()
                {
                    TacticsModifiers = new Modifiers(0.05f, 0.05f),
                    LeadershipModifiers = new Modifiers(0.05f, 0.05f),
                },
                NumbersAdvantageModifier = new NumbersAdvantageModifier()
                {
                    HighCap = 30,
                    LowCap = 30
                },
                DoesntMakeSenseToAttackModifier = -90
            };
        }
    }
}
