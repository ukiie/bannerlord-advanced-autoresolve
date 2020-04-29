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

        internal static void AddModelToSimulations(SimulationModel simulationModel)
        {
            if(!pool.TryAdd(simulationModel.BattleId, simulationModel))
            {
#if DEBUG
                MessageHelper.DisplayText($"{simulationModel.BattleId} Could not add a battle to advanced simulation! Will use default simulation instead", DisplayTextStyle.Warning);
#endif
            }
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
