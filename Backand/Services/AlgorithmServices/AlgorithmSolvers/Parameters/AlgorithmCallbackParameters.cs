using Google.OrTools.Sat;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;

/// <summary>
/// Параметры для ответа алгоритма.
/// </summary>
/// <param name="MaterialsInStoragesAssignsVariablesMatrix">Матрица выбраных материалов в складе.</param>
/// <param name="StorageAssignsVariablesVector">Матрица выбраных складов.</param>
/// <param name="TransportsToStoragesAssignsVariablesMatrix">Матрица выбранных транспортов до склада.</param>
/// <param name="StoragesToNonGroundTransportVariablesMatrix">Матрица выбранных складов до транспорта.</param>
public record AlgorithmCallbackParameters(
    BoolVar[,] MaterialsInStoragesAssignsVariablesMatrix,
    BoolVar[] StorageAssignsVariablesVector,
    BoolVar[,] TransportsToStoragesAssignsVariablesMatrix,
    BoolVar[,] StoragesToNonGroundTransportVariablesMatrix);
