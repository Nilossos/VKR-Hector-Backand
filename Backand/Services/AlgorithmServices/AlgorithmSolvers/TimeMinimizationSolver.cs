using Backand.FrontendEntities.Requests;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Abstraction;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Attributes;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;
using Google.OrTools.Sat;
using System.Diagnostics;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers;

/// <summary>
/// Алгоритм минимизации времени.
/// </summary>
[SolverFilter(TargetMark.Time)]
public class TimeMinimizationSolver : CpSatAlgorithmBaseSolver
{
    public TimeMinimizationSolver(AlgorithmSolverParameters parameters) : base(parameters) { }

    public override void SetMinimization()
    {
        var times = new List<LinearExpr>();

        for (var i = 0; i < TransportsToStoragesCount; i++)
        {
            for (var j = 0; j < StoragesCount; j++)
            {
                var distance = TransportsToStoragesAssignVariableMatrix[i, j] * DistanceTransportStorageMatrix[i, j];
                var timeToStorageVariable = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}");
                Model.AddDivisionEquality(timeToStorageVariable, distance, GroundTransportInfos[i].AverageSpeed);


                var endGroundDistance = IsGroundDeliveryVariable * DistanceStorageObjectVector[j];
                var timeToObjectFromStorageVariable = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}_end");
                Model.AddDivisionEquality(timeToObjectFromStorageVariable, endGroundDistance, GroundTransportInfos[i].AverageSpeed);


                for (var z = 0; z < StoragesToNotGroundTransportsCount; z++)
                {
                    var toNonGroundDistance = StoragesToNotGroundTransportsAssignVariableMatrix[j, z] * DistanceStorageNotGroundTransportsMatrix[j, z];
                    var timeStorageToNonGround = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}_{z}_to_non_ground");
                    Model.AddDivisionEquality(timeStorageToNonGround, toNonGroundDistance, GroundTransportInfos[i].AverageSpeed);

                    var nonGroundDistance = DistanceTransportObjectVector[z];
                    var timeTransportToObject = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}_{z}_air");
                    Model.AddDivisionEquality(timeTransportToObject, nonGroundDistance, NonGroundTransportInfos[z].AverageSpeed);

                    Model.Add(timeStorageToNonGround == 0).OnlyEnforceIf(IsGroundDeliveryVariable);
                    Model.Add(timeTransportToObject == 0).OnlyEnforceIf(IsGroundDeliveryVariable);

                    // Время пути для данного склада и транспорта
                    var totalTimeInPath = timeToStorageVariable + timeStorageToNonGround + timeTransportToObject + timeToObjectFromStorageVariable;

                    times.Add(totalTimeInPath);
                }

                if (StoragesToNotGroundTransportsCount == 0)
                {
                    var totalGroundTime = timeToStorageVariable + timeToObjectFromStorageVariable;
                    times.Add(totalGroundTime);
                }
            }
        }

        var totalTime = LinearExpr.Sum(times);

        Model.Minimize(totalTime);
    }
}