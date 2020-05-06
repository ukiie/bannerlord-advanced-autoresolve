using AdvancedAutoResolve.Helpers;
using AdvancedAutoResolve.Simulation.Models;
using System;
using System.Collections.Concurrent;
using TaleWorlds.ObjectSystem;

namespace AdvancedAutoResolve.Simulation
{
    internal class SimulationsPool
    {
        private readonly static ConcurrentDictionary<MBGUID, SimulationModel> pool = new ConcurrentDictionary<MBGUID, SimulationModel>();

        internal static bool AddModelToSimulations(SimulationModel simulationModel)
        {
            bool added = pool.TryAdd(simulationModel.BattleId, simulationModel);
            if (!added)
            {
                if (Config.CurrentConfig.ShouldLogThis(simulationModel.IsPlayerInvolved))
                {
                    if (pool.TryGetValue(simulationModel.BattleId, out _))
                    {
                        MessageHelper.DisplayText($"{simulationModel.EventDescription} Already added.", DisplayTextStyle.Warning);
                    }
                    else
                    {
                        MessageHelper.DisplayText($"{simulationModel.EventDescription} Could not add a battle to advanced simulation! Will use default simulation instead", DisplayTextStyle.Warning);
                    }
                }
            }
            return added;
        }

        internal static bool TryGetSimulationModel(MBGUID battleId, out SimulationModel simulationModel)
        {
            return pool.TryGetValue(battleId, out simulationModel);
        }

        internal static bool TryRemoveSimulationModel(MBGUID battleId, out SimulationModel simulationModel)
        {
            return pool.TryRemove(battleId, out simulationModel);
        }
    }
}
