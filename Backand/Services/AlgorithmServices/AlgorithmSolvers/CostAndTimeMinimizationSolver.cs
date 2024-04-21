using Backand.FrontendEntities.Requests;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Abstraction;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Attributes;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;
using Google.OrTools.Sat;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers;

/// <summary>
/// Алгоритм минимизации цены и времени.
/// </summary>
[SolverFilter(FilterMethod.Balanced)]
public class CostAndTimeMinimizationSolver : CpSatAlgorithmBaseSolver
{
    public CostAndTimeMinimizationSolver(AlgorithmSolverParameters parameters) : base(parameters) { }
    
    public override void SetMinimization()
    {
        var costs = new List<LinearExpr>();
        var times = new List<LinearExpr>();
        
        for (var i = 0; i < TransportsToStoragesCount; i++)
        {
            for (var j = 0; j < StoragesCount; j++)
            {
                var distance = TransportsToStoragesAssignVariableMatrix[i, j] * DistanceTransportStorageMatrix[i, j];
                
                var startDeliveryCost = distance
                                        * GroundTransportInfos[i].Coefficient;
                
                var timeToStorageVariable = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}");
                
                Model.AddDivisionEquality(timeToStorageVariable, distance, GroundTransportInfos[i].AverageSpeed);
                
                var deliveryCostTransportToObject = new LinearExpr[StoragesToNotGroundTransportsCount];
                var timeTransportToObjects = new LinearExpr[StoragesToNotGroundTransportsCount];
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
                    
                    deliveryCostTransportToObject[z] = toNonGroundDistance * GroundTransportInfos[i].Coefficient +
                                                       nonGroundDistance * NonGroundTransportInfos[z].Coefficient;
                    timeTransportToObjects[z] = timeTransportToObject + timeStorageToNonGround;
                }
                
                var timesStorageToTransportsSum = LinearExpr.Sum(timeTransportToObjects) + 0;
                Model.Add(timesStorageToTransportsSum == 0).OnlyEnforceIf(IsGroundDeliveryVariable);
                
                var nonGroundsDeliveryCostsSum = LinearExpr.Sum(deliveryCostTransportToObject) + 0;
                Model.Add(nonGroundsDeliveryCostsSum == 0).OnlyEnforceIf(IsGroundDeliveryVariable);
                
                var endGroundDistance = IsGroundDeliveryVariable * DistanceStorageObjectVector[j];


                var totalDeliveryCost = startDeliveryCost + nonGroundsDeliveryCostsSum +
                                        endGroundDistance * GroundTransportInfos[i].Coefficient;

                var timeToObjectFromStorageVariable = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}_end");
                
                Model.AddDivisionEquality(timeToObjectFromStorageVariable, endGroundDistance,
                    GroundTransportInfos[i].AverageSpeed);

                var totalTimeInPath = timeToStorageVariable + timesStorageToTransportsSum + timeToObjectFromStorageVariable;
                
                times.Add(totalTimeInPath);
                
                var materialsCostsInStorage = new LinearExpr[MaterialsCount]; 
                for (var z = 0; z < MaterialsCount; z++)
                {
                    materialsCostsInStorage[z] = MaterialInStorageAssignVariableMatrix[j, z] * (
                        (MaterialParams[j, z] == null) ? 0 : MaterialParams[j, z]!.PricePerUnit);
                }
                var totalMaterialsCosts = LinearExpr.Sum(materialsCostsInStorage); 
                costs.Add(totalDeliveryCost + totalMaterialsCosts);
            }
        }

        var totalCost = LinearExpr.Sum(costs);
        var totalTime = LinearExpr.Sum(times);

        Model.Minimize(totalCost);
        Model.Minimize(totalTime);
    }
}