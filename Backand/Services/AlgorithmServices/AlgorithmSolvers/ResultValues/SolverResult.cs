using System.Collections;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers.ResultValues;

/// <summary>
/// Решение алгоритма.
/// </summary>
public class SolverResult : IEnumerable<SolverResultInfo>
{
    /// <summary>
    /// Коллекция информации.
    /// </summary>
    public IList<SolverResultInfo> AlgorithmResultInfos { get; set; } = new List<SolverResultInfo>();

    public void Add(SolverResultInfo resultInfo) => AlgorithmResultInfos.Add(resultInfo);
    
    public IEnumerator<SolverResultInfo> GetEnumerator() => AlgorithmResultInfos.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}