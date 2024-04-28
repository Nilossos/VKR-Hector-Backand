namespace Backand.Services.AlgorithmServices.AlgorithmSolvers.ResultValues;

/// <summary>
/// Информация решения алгоритма.
/// </summary>
public class SolverResultInfo
{
    /// <summary>
    /// Индекст материала во входном массиве.
    /// </summary>
    public int MaterialIndex { get; set; }
    
    /// <summary>
    /// Индекс склада во входном массиве.
    /// </summary>
    public int StorageIndex { get; set; }
    
    /// <summary>
    /// Индекс наземного транспорта во входном массиве.
    /// </summary>
    public int GroundTransportIndex { get; set; }
    
    /// <summary>
    /// Индекс НЕ наземного траспорта во входном массиве.
    /// </summary>
    public int NonGroundTransportIndex { get; set; } = -1;
}