using Backand.FrontendEntities.Requests;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Abstraction;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Attributes;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;
using Google.OrTools.Sat;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers;

/// <summary>
/// Алгоритм балансовой минимизации.
/// </summary>
[SolverFilter(FilterMethod.Balanced)]
public class CostAndTimeMinimizationSolver : CpSatAlgorithmBaseSolver
{
    public CostAndTimeMinimizationSolver(AlgorithmSolverParameters parameters) : base(parameters) { }
    
    public override void SetMinimization()
    {
        var times = new List<LinearExpr>();
       var costs = new List<LinearExpr>(); 
        
        for (var i = 0; i < TransportsToStoragesCount; i++)
        {
            for (var j = 0; j < StoragesCount; j++)
            {
                var startDistance = TransportsToStoragesAssignVariableMatrix[i, j]
                                    * DistanceTransportStorageMatrix[i, j];

                var startDeliveryCost = startDistance * GroundTransportInfos[i].Coefficient;
                
                var timeToStorageVariable = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}");
                Model.AddDivisionEquality(timeToStorageVariable, startDistance, GroundTransportInfos[i].AverageSpeed);
                
                var deliveryCostTransportToObject = new LinearExpr[StoragesToNotGroundTransportsCount];
                var timesTransportToObject = new LinearExpr[StoragesToNotGroundTransportsCount];
                for (var z = 0; z < StoragesToNotGroundTransportsCount; z++)
                {
                    // проверить едет ли TransportsToStoragesCount до этого StoragesToNotGroundTransportsCount (второго логиста)

                    var isAssignedNonGroundTransport = Model.NewBoolVar($"non_ground_assigned_{i}_{j}_{z}");
                    
                    Model.AddMultiplicationEquality(isAssignedNonGroundTransport,
                        StoragesToNotGroundTransportsAssignVariableMatrix[j, z],
                        IsGroundDeliveryVariable.NotAsExpr());
                    
                    var toNonGroundDistance = isAssignedNonGroundTransport *
                                              DistanceStorageNotGroundTransportsMatrix[j, z];
                    var deliveryCostToNonGround = toNonGroundDistance * GroundTransportInfos[i].Coefficient;
                    
                    var nonGroundDistance = isAssignedNonGroundTransport *
                                            DistanceTransportObjectVector[z];
                    var nonGroundDeliveryCost = nonGroundDistance * NonGroundTransportInfos[z].Coefficient;

                    deliveryCostTransportToObject[z] = deliveryCostToNonGround + nonGroundDeliveryCost;
                    
                    var timeTransportToObject = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}_{z}_air");
                    Model.AddDivisionEquality(timeTransportToObject, nonGroundDistance, NonGroundTransportInfos[z].AverageSpeed);
                    var timeStorageToNonGround = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}_{z}_to_non_ground");
                    Model.AddDivisionEquality(timeStorageToNonGround, toNonGroundDistance, GroundTransportInfos[i].AverageSpeed);
                    timesTransportToObject[z] = timeTransportToObject + timeStorageToNonGround;
                }
                var timesStorageToTransportsSum = LinearExpr.Sum(timesTransportToObject);
                
                var nonGroundsDeliveryCostsSum = LinearExpr.Sum(deliveryCostTransportToObject) + 0;

                var isEndGroundDelivery = Model.NewBoolVar($"is_end_ground_delivery_{i}_{j}");

                Model.AddMultiplicationEquality(isEndGroundDelivery, IsGroundDeliveryVariable,
                    TransportsToStoragesAssignVariableMatrix[i, j]);
                
                var endGroundDistance = isEndGroundDelivery * DistanceStorageObjectVector[j];
                
                var totalDeliveryCost = startDeliveryCost + nonGroundsDeliveryCostsSum + endGroundDistance;

                var materialsCostsInStorage = new LinearExpr[MaterialsCount]; 
                for (var z = 0; z < MaterialsCount; z++)
                {
                    if(MaterialParams[j, z] != null) materialsCostsInStorage[z] =
                        MaterialParams[j, z]!.Cost * MaterialInStorageAssignVariableMatrix[j, z];
                    materialsCostsInStorage[z] = MaterialInStorageAssignVariableMatrix[j, z] + (int.MaxValue / 2);
                }
                var totalMaterialsCosts = LinearExpr.Sum(materialsCostsInStorage); 
                
                var timeToObjectFromStorageVariable = Model.NewIntVar(0, int.MaxValue / 3, $"time_{i}_{j}_end");
                    
                Model.AddDivisionEquality(timeToObjectFromStorageVariable, endGroundDistance, GroundTransportInfos[i].AverageSpeed);

                var totalTimeInPath = timeToStorageVariable + timesStorageToTransportsSum + timeToObjectFromStorageVariable;
                
                times.Add(totalTimeInPath);
                
                costs.Add(totalDeliveryCost + totalMaterialsCosts);
            }
        }

        var totalTime = LinearExpr.Sum(times);
        var totalCost = LinearExpr.Sum(costs);

        Model.Minimize(totalTime);
        Model.Minimize(totalCost);
    }
}