using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using HarmonyLib;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem;
using static TaleWorlds.CampaignSystem.MapEvent;
using AdvancedAutoResolve.Configuration;
using AdvancedAutoResolve.Helpers;

namespace AdvancedAutoResolve
{
    internal class SubModule : MBSubModuleBase
    {
        internal static Random Random;
        protected override void OnSubModuleLoad()
        {
            Harmony harmony = new Harmony("com.ukie.advanced-autoresolve");
            harmony.PatchAll(typeof(SubModule).Assembly);
            Random = new Random();
            Config.InitializeConfig();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (Config.ConfigLoaded)
            {
                MessageHelper.DisplayText("Loaded AdvancedAutoresolve", DisplayTextStyle.Success);
            }
            else
            {
                MessageHelper.DisplayText(Config.ConfigError, DisplayTextStyle.Warning);
            }
        }

        public override void OnCampaignStart(Game game, object starterObject)
        {
            if (game.GameType is Campaign && starterObject is CampaignGameStarter starter)
            {
                AddBehaviors(starter);
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (game.GameType is Campaign && gameStarterObject is CampaignGameStarter starter)
            {
                AddBehaviors(starter);
            }
        }

        private void AddBehaviors(CampaignGameStarter gameInitializer)
        {
            gameInitializer.AddBehavior(new AdvancedAutoResolveLogic());
        }
    }
}
