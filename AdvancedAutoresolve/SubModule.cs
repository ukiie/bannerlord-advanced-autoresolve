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
    public class SubModule : MBSubModuleBase
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

        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            if (mission.CombatType == Mission.MissionCombatType.Combat)
            {
                var battleObserverBehavior = mission.GetMissionBehaviour<BattleObserverMissionLogic>();
                if (battleObserverBehavior != null)
                {
                    battleObserverBehavior.SetObserver(new SimulationObserver());
                }

            }
            base.OnMissionBehaviourInitialize(mission);
        }

        internal static bool IsValidEventType(BattleTypes battleType)
        {
            return battleType == BattleTypes.FieldBattle;
        }
    }
}
