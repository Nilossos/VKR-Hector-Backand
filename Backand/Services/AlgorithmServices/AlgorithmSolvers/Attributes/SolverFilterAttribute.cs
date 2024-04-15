using Backand.FrontendEntities.Requests;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers.Attributes;

/// <summary>
/// Фильтр для решения.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SolverFilterAttribute : Attribute
{
    public FilterMethod FilterMethod { get; }

    public SolverFilterAttribute(FilterMethod filterMethod) => FilterMethod = filterMethod;
}