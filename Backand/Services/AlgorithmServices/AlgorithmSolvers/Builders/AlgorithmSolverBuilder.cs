using System.Linq.Expressions;
using System.Reflection;
using Backand.FrontendEntities.Requests;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Abstraction;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Attributes;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers.Builders;

/// <summary>
/// Builder алгоритмов.
/// </summary>
public class AlgorithmSolverBuilder
{
    private readonly AlgorithmSolverParameters _parameters;

    /// <summary>
    /// Конструкторы алгоритмов.
    /// </summary>
    /// 
    //каким то образом словарь собирает в себя по ключу нужные функции в методе GetAlgorithmsConstructorsDictionaryFromAssembly
    private static readonly Dictionary<FilterMethod, Func<AlgorithmSolverParameters, CpSatAlgorithmBaseSolver>>
        AlgorithmsConstructorsDictionary = GetAlgorithmsConstructorsDictionaryFromAssembly(Assembly.GetExecutingAssembly());

    public AlgorithmSolverBuilder(AlgorithmSolverParameters parameters) => _parameters = parameters;

    //Здесь solver попадает функция, которая хранится в AlgorithmsConstructorsDictionary. Функция выбирается в зависимости от ключа (filterMethod)
    //у функции вызывается родительский SetMinimization. В реальности отработает одна из этих: TimeMinimizationSolver, CostMinimizationSolver или TimeAndConstMinimizationSolver
    public CpSatAlgorithmBaseSolver BuildSolverFromFilterMethod(FilterMethod filterMethod)
    {
        var solver = AlgorithmsConstructorsDictionary[filterMethod](_parameters);
        solver.SetMinimization();
        return solver;
    }

    private static Dictionary<FilterMethod, Func<AlgorithmSolverParameters, CpSatAlgorithmBaseSolver>>
        GetAlgorithmsConstructorsDictionaryFromAssembly(Assembly assembly)
    {
        var attributesAndSolversTypes = assembly
            .GetTypes()
            .Where(type => type.CustomAttributes
                .Any(a => a.AttributeType == typeof(SolverFilterAttribute)))
            .Select(type => Tuple.Create(type.GetCustomAttribute<SolverFilterAttribute>(), type));

        var result = new Dictionary<FilterMethod, Func<AlgorithmSolverParameters, CpSatAlgorithmBaseSolver>>();

        foreach (var attributeAndSolverType in attributesAndSolversTypes)
        {
            var attributeFilterMethod = attributeAndSolverType.Item1!.FilterMethod;
            var solverType = attributeAndSolverType.Item2!;
            if(result.ContainsKey(attributeFilterMethod)) continue;
            var solverConstructors = solverType.GetConstructors();
            if(solverConstructors.Length == 0) continue;
            var solverConstructor = solverConstructors.First();
            var constructorParameterExpression =
                Expression.Parameter(typeof(AlgorithmSolverParameters), "SolverParameters");
            var constructorExpression = Expression.New(solverConstructor, constructorParameterExpression);
            var constructorLambda = 
                Expression.Lambda<Func<AlgorithmSolverParameters, CpSatAlgorithmBaseSolver>>(constructorExpression,
                constructorParameterExpression);
            var constructorFunction = constructorLambda.Compile();
            result[attributeFilterMethod] = constructorFunction;
        }
        return result;
    }
}