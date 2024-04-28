using Backand.Services.AlgorithmServices.AlgorithmPreparers.Values;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;

/// <summary>
/// Параметры решителя алгоритма.
/// </summary>
/// <param name="DistanceStorageObjectVector">Расстояния. Склад -> Объект.</param>
/// <param name="DistanceTransportStorageMatrix">Расстояния. Транспорт -> Склад</param>
/// <param name="DistanceStorageNotGroundTransportsMatrix">Расстояния. Склад -> Транспорт.</param>
/// <param name="DistanceTransportObjectVector">Растояния. Транспорт -> Объект.</param>
/// <param name="GroundTransportInfos">Информация о наземных транспортах.</param>
/// <param name="NonGroundTransportInfos">Информация о не наземных транспортах.</param>
/// <param name="MaterialParams">Параметры материалов.</param>
/// <param name="DeliveryTypes">Типы доставки материалов.</param>
public record AlgorithmSolverParameters(
    long[] DistanceStorageObjectVector,
    long[,] DistanceTransportStorageMatrix,
    long[,] DistanceStorageNotGroundTransportsMatrix,
    long[] DistanceTransportObjectVector,
    LongTransportInfo[] GroundTransportInfos,
    LongTransportInfo[] NonGroundTransportInfos,
    LongMaterialParams?[,] MaterialParams,
    int[] DeliveryTypes);