using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using HarmonyLib;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem;
using static TaleWorlds.CampaignSystem.MapEvent;

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
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            InformationManager.DisplayMessage(new InformationMessage("Loaded AdvancedAutoresolve", new Color(0, 1, 0)));
        }

        internal static bool IsValidEventType(BattleTypes battleType)
        {
            return battleType == BattleTypes.FieldBattle;
        }

        public override void OnCampaignStart(Game game, object starterObject)
        {
            if (game.GameType is Campaign)
            {
                CampaignGameStarter gameInitializer = (CampaignGameStarter)starterObject;
                AddBehaviors(gameInitializer);
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (game.GameType is Campaign)
            {
                CampaignGameStarter gameStarterObject2 = (CampaignGameStarter)gameStarterObject;
                AddBehaviors(gameStarterObject2);
            }
        }

        private void AddBehaviors(CampaignGameStarter gameInitializer)
        {
            gameInitializer.AddBehavior(new AdvancedAutoResolveLogic());
        }
    }
}
