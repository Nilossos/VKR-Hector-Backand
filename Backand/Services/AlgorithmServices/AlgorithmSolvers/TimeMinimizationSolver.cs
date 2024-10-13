using Backand.FrontendEntities.Requests;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Abstraction;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Attributes;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;
using Google.OrTools.Sat;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers;

/// <summary>
/// Алгоритм минимизации времени.
/// </summary>
[SolverFilter(FilterMethod.Time)]
public class TimeMinimizationSolver : CpSatAlgorithmBaseSolver
{
    public TimeMinimizationSolver(AlgorithmSolverParameters parameters) : base(parameters) { }
    
    //вроде бы здесь идет заполнение таблиц Model, из которых она будет искать оптимум. Еще проверю
    public override void SetMinimization()
    {
        var times = new List<LinearExpr>(); 
        
        for (var i = 0; i < TransportsToStoragesCount; i++)
        {
            for (var j = 0; j < StoragesCount; j++)
            {
                var distance = TransportsToStoragesAssignVariableMatrix[i, j]
                                     * DistanceTransportStorageMatrix[i, j];
                
                var timeToStorageVariable = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}");
                
                Model.AddDivisionEquality(timeToStorageVariable, distance, GroundTransportInfos[i].AverageSpeed);

                var timesTransportToObject = new LinearExpr[StoragesToNotGroundTransportsCount];
                for (var z = 0; z < StoragesToNotGroundTransportsCount; z++)
                {
                    var toNonGroundDistance = IsGroundDeliveryVariable.NotAsExpr() *
                                              DistanceStorageNotGroundTransportsMatrix[j, z];
                    
                    var nonGroundDistance = IsGroundDeliveryVariable.NotAsExpr() *
                                            DistanceTransportObjectVector[z];
                    
                    var timeTransportToObject = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}_{z}_air");
                    Model.AddDivisionEquality(timeTransportToObject, nonGroundDistance, NonGroundTransportInfos[z].AverageSpeed);
                    
                    var timeStorageToNonGround = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}_{z}_to_non_ground");
                    Model.AddDivisionEquality(timeStorageToNonGround, toNonGroundDistance, GroundTransportInfos[i].AverageSpeed);
                    
                    timesTransportToObject[z] = timeTransportToObject + timeStorageToNonGround;
                }
                
                var timesStorageToTransportsSum = LinearExpr.Sum(timesTransportToObject) + 0;
                Model.Add(timesStorageToTransportsSum == 0).OnlyEnforceIf(IsGroundDeliveryVariable);
                
                var endGroundDistance = IsGroundDeliveryVariable * DistanceStorageObjectVector[j];
                
                var timeToObjectFromStorageVariable = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}_end");
                    
                Model.AddDivisionEquality(timeToObjectFromStorageVariable, endGroundDistance, GroundTransportInfos[i].AverageSpeed);

                var totalTimeInPath = timeToStorageVariable + timesStorageToTransportsSum + timeToObjectFromStorageVariable;
                
                times.Add(totalTimeInPath);
            }
        }

        var totalTime = LinearExpr.Sum(times);

        Model.Minimize(totalTime);
    }
}