using Backand.FrontendEntities.Requests;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Abstraction;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Attributes;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;
using Google.OrTools.Sat;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers;

/// <summary>
/// Алгоритм минимизации цены и времени.
/// </summary>
[SolverFilter(TargetMark.Money)]
public class CostMinimizationSolver : CpSatAlgorithmBaseSolver
{
    public CostMinimizationSolver(AlgorithmSolverParameters parameters) : base(parameters) { }
    
    public override void SetMinimization()
    {
        var costs = new List<LinearExpr>(); 
        
        for (var i = 0; i < TransportsToStoragesCount; i++)
        {
            for (var j = 0; j < StoragesCount; j++)
            {
                var startDeliveryCost = TransportsToStoragesAssignVariableMatrix[i, j]
                                     * DistanceTransportStorageMatrix[i, j]
                                     * GroundTransportInfos[i].Coefficient;
                
                var deliveryCostTransportToObject = new LinearExpr[StoragesToNotGroundTransportsCount];
                for (var z = 0; z < StoragesToNotGroundTransportsCount; z++)
                {
                    var toNonGroundDistance = IsGroundDeliveryVariable.NotAsExpr() *
                                              DistanceStorageNotGroundTransportsMatrix[j, z];
                    
                    var nonGroundDistance = IsGroundDeliveryVariable.NotAsExpr() *
                                            DistanceTransportObjectVector[z];
                    
                    deliveryCostTransportToObject[z] = toNonGroundDistance * GroundTransportInfos[i].Coefficient +
                                                       nonGroundDistance * NonGroundTransportInfos[z].Coefficient;
                }
                
                var nonGroundsDeliveryCostsSum = LinearExpr.Sum(deliveryCostTransportToObject) + 0;
                Model.Add(nonGroundsDeliveryCostsSum == 0).OnlyEnforceIf(IsGroundDeliveryVariable);
                
                var endGroundDistance = IsGroundDeliveryVariable * DistanceStorageObjectVector[j] * GroundTransportInfos[i].Coefficient;
                
                var totalDeliveryCost = startDeliveryCost + nonGroundsDeliveryCostsSum + endGroundDistance;

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

        Model.Minimize(totalCost);
    }
}