using Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.ResultValues;
using Google.OrTools.Sat;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers.Callbacks;

/// <summary>
/// Ответ алгоритма.
/// </summary>
public class AlgorithmCallback : CpSolverSolutionCallback
{
    private readonly AlgorithmCallbackParameters _parameters;

    public AlgorithmCallback(AlgorithmCallbackParameters parameters) => _parameters = parameters;
    
    public SolverResult Result { get; set; } = new();
    
    public override void OnSolutionCallback()
    {
        Result = new SolverResult();
        
        var (materialsInStoragesAssignsVariablesMatrix,
            storageAssignsVariablesVector,
            transportsToStoragesAssignsVariablesMatrix,
            storagesToNonGroundTransportVariablesMatrix) = _parameters;

        var materialsCount = materialsInStoragesAssignsVariablesMatrix.GetLength(1);
        var storagesCount = storageAssignsVariablesVector.Length;
        var transportsToStoragesCount = transportsToStoragesAssignsVariablesMatrix.GetLength(0);
        var storagesToNonGroundTransportsCount = storagesToNonGroundTransportVariablesMatrix.GetLength(1);
        
        for (var materialIndex = 0;
             materialIndex < materialsCount;
             materialIndex++)
        {
            for (var storageIndex = 0;
                 storageIndex < storagesCount;
                 storageIndex++)
            {
                if(Value(materialsInStoragesAssignsVariablesMatrix[storageIndex, materialIndex]) != 1) continue;
                var resultInfo = new SolverResultInfo
                {
                    MaterialIndex = materialIndex,
                    StorageIndex = storageIndex
                };

                for (var transportToStorageIndex = 0;
                     transportToStorageIndex < transportsToStoragesCount;
                     transportToStorageIndex++)
                {
                    if(Value(transportsToStoragesAssignsVariablesMatrix[transportToStorageIndex, storageIndex]) != 1) continue;
                    resultInfo.GroundTransportIndex = transportToStorageIndex;
                }
                
                for (var storageToTransportIndex = 0;
                     storageToTransportIndex < storagesToNonGroundTransportsCount;
                     storageToTransportIndex++)
                {
                    if(Value(storagesToNonGroundTransportVariablesMatrix[storageIndex, storageToTransportIndex]) != 1) continue;
                    resultInfo.NonGroundTransportIndex = storageToTransportIndex;
                }
                
                Result.Add(resultInfo);
            }
        }
    }
}