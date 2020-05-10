using AdvancedAutoResolve.Configuration;
using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Models;
using AdvancedAutoResolve.Simulation;
using AdvancedAutoResolve.Simulation.Models;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment.Managers;
using TaleWorlds.Core;

namespace AdvancedAutoResolve.HarmonyPatches.MapEventSidePatches
{
    [HarmonyPatch(typeof(MapEventSide), nameof(ApplySimulationDamageToSelectedTroop))]
    internal static class ApplySimulationDamageToSelectedTroop
    {
        internal static bool Prefix(ref bool __result, ref MapEventSide __instance, ref CharacterObject ____selectedSimulationTroop, ref UniqueTroopDescriptor ____selectedSimulationTroopDescriptor, int damage, DamageTypes damageType, out int troopState, PartyBase strikerParty)
        {
            if (strikerParty.MapEvent != null && SimulationModel.IsValidEventType(strikerParty.MapEvent.EventType))
            {
                if (SimulationsPool.TryGetSimulationModel(strikerParty.MapEvent.Id, out var simulationModel))
                {
                    SimulationTroopState simulationTroopState = SimulationTroopState.Alive;
                    __result = false;
                    
                    // try to find the attacked troop in our model. Id doesn't exist call vanilla method
                    var troopId = ____selectedSimulationTroop.Id;
                    var troop = simulationModel.Parties[(int)__instance.MissionSide].Troops.Find(t => t.CharacterObject.Id == troopId);
                    if (troop == null)
                    {
                        troopState = (int)simulationTroopState;
                        return true;
                    }

                    var battleObserver = MapEventSideAccessTools.GetBattleObserver(__instance);
                    var allocatedTroops = MapEventSideAccessTools.GetAllocatedTroops(__instance);

                    // troop is a Hero logic
                    if (____selectedSimulationTroop.IsHero)
                    {
                        __instance.AddHeroDamage(____selectedSimulationTroop.HeroObject, damage);
                        if (____selectedSimulationTroop.HeroObject.IsWounded)
                        {
                            __result = true;
                            simulationTroopState = SimulationTroopState.Wounded;
                            if (battleObserver != null)
                            {
                                battleObserver.TroopNumberChanged(__instance.MissionSide, __instance.GetAllocatedTroopParty(____selectedSimulationTroopDescriptor), ____selectedSimulationTroop, -1, 0, 1, 0, 0, 0);
                            }
                        }
                    }
                    // regular logic
                    else if(troop.ApplyDamage(damage))
                    {
                        PartyBase party = allocatedTroops[____selectedSimulationTroopDescriptor].Party;
                        float survivalChance = Campaign.Current.Models.PartyHealingModel.GetSurvivalChance(party, ____selectedSimulationTroop, damageType, strikerParty);

                        if (MBRandom.RandomFloat < survivalChance)
                        {
                            __instance.OnTroopWounded(____selectedSimulationTroopDescriptor);
                            simulationTroopState = SimulationTroopState.Wounded;
                            if (battleObserver != null)
                            {
                                battleObserver.TroopNumberChanged(__instance.MissionSide, __instance.GetAllocatedTroopParty(____selectedSimulationTroopDescriptor), ____selectedSimulationTroop, -1, 0, 1, 0, 0, 0);
                            }
                            SkillLevelingManager.OnSurgeryApplied(party.MobileParty, 1f);
                        }
                        else
                        {
                            __instance.OnTroopKilled(____selectedSimulationTroopDescriptor);
                            simulationTroopState = SimulationTroopState.Kille;

                            if (battleObserver != null)
                            {
                                battleObserver.TroopNumberChanged(__instance.MissionSide, __instance.GetAllocatedTroopParty(____selectedSimulationTroopDescriptor), ____selectedSimulationTroop, -1, 1, 0, 0, 0, 0);
                            }
                            SkillLevelingManager.OnSurgeryApplied(party.MobileParty, 0.5f);
                        }
                        __result = true;
                    }

                    if (__result)
                    {
                        __instance.RemoveSelectedTroopFromSimulationList();
                    }

                    troopState = (int)simulationTroopState;
                    return false;
                }
            }
            troopState = 1;
            return true;
        }
    }
}
