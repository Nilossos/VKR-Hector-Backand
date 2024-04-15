using Backand.DbEntities;
using Backand.Services.AlgorithmServices.AlgorithmPreparers.Values;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Callbacks;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.ResultValues;
using Google.OrTools.Sat;

namespace Backand.Services.AlgorithmServices.AlgorithmSolvers.Abstraction;

/// <summary>
/// База решения минимизации.
/// </summary>
public abstract class CpSatAlgorithmBaseSolver
{
    /// <summary>
    /// Модель.
    /// </summary>
    protected readonly CpModel Model = new();
    
    /// <summary>
    /// Решитель.
    /// </summary>
    protected readonly CpSolver Solver = new();

    protected long[] DistanceStorageObjectVector;
    protected long[,] DistanceTransportStorageMatrix;
    protected long[,] DistanceStorageNotGroundTransportsMatrix;
    protected long[] DistanceTransportObjectVector;
    protected LongTransportInfo[] GroundTransportInfos;
    protected LongTransportInfo[] NonGroundTransportInfos;
    protected LongMaterialParams?[,] MaterialParams;
    protected int[] DeliveryTypes;

    protected readonly int StoragesCount;
    protected readonly int TransportsToStoragesCount;
    protected readonly int StoragesToNotGroundTransportsCount;
    protected readonly int MaterialsCount;

    protected BoolVar IsGroundDeliveryVariable;
    protected readonly BoolVar[] StorageAssignVariableVector;
    protected readonly BoolVar[,] TransportsToStoragesAssignVariableMatrix;
    protected readonly BoolVar[,] MaterialInStorageAssignVariableMatrix;
    protected readonly BoolVar[,] StoragesToNotGroundTransportsAssignVariableMatrix;

    public CpSatAlgorithmBaseSolver(AlgorithmSolverParameters parameters)
    {
        (DistanceStorageObjectVector,
            DistanceTransportStorageMatrix,
            DistanceStorageNotGroundTransportsMatrix,
            DistanceTransportObjectVector,
            GroundTransportInfos,
            NonGroundTransportInfos,
            MaterialParams,
            DeliveryTypes) = parameters;

        StoragesCount = DistanceStorageObjectVector.Length;
        TransportsToStoragesCount = DistanceTransportStorageMatrix.GetLength(0);
        MaterialsCount = MaterialParams.GetLength(1);
        StoragesToNotGroundTransportsCount = DistanceStorageNotGroundTransportsMatrix.GetLength(1);
        StorageAssignVariableVector = new BoolVar[StoragesCount];
        TransportsToStoragesAssignVariableMatrix = new BoolVar[TransportsToStoragesCount, StoragesCount];
        MaterialInStorageAssignVariableMatrix = new BoolVar[StoragesCount, MaterialsCount];
        StoragesToNotGroundTransportsAssignVariableMatrix =
            new BoolVar[StoragesCount, StoragesToNotGroundTransportsCount];
        InitTransportTypeConstrains();
        InitStorageAssignVariablesVector();
        InitGroundTransportToStoragesVariablesAndConstrains();
        InitGroundTransportsToStoragesAssignsConstrains();
        InitMaterialAssignVariables();
        InitMaterialsConstraints();
        InitStoragesToNonGroundTransportsVariablesAndConstrains();
        InitStoragesToNonGroundTransportsConstrains();
    }
    
    /// <summary>
    /// Получить результат.
    /// </summary>
    /// <returns>Результат алгоритма.</returns>
    public SolverResult GetResult()
    {
        var callBackParameters = new AlgorithmCallbackParameters(
            MaterialInStorageAssignVariableMatrix,
            StorageAssignVariableVector,
            TransportsToStoragesAssignVariableMatrix,
            StoragesToNotGroundTransportsAssignVariableMatrix);

        var callback = new AlgorithmCallback(callBackParameters);
        
        var algorithmResult = new SolverResult();

        return Solver.Solve(Model, callback) != CpSolverStatus.Optimal ? algorithmResult : callback.Result;
    }
    
    /// <summary>
    /// Инициализировать ограничения типа транспорта.
    /// </summary>
    private void InitTransportTypeConstrains()
    {
        IsGroundDeliveryVariable = Model.NewBoolVar($"is_only_ground_delivery");

        if (!DeliveryTypes.Contains((int)TransportTypeValue.Ground))
            Model.Add(IsGroundDeliveryVariable == 0);
        else if (DeliveryTypes.Contains((int)TransportTypeValue.Ground) && DeliveryTypes.Length == 1)
            Model.Add(IsGroundDeliveryVariable == 1);
    }
    
    /// <summary>
    ///  Инициализировать переменные складов.
    /// </summary>
    private void InitStorageAssignVariablesVector()
    {
        for (var i = 0; i < StoragesCount; i++)
            StorageAssignVariableVector[i] = Model.NewBoolVar($"storage_{i}_assign");
    }

    /// <summary>
    /// Инициализировать переменные и ограничения наземного транспорта.
    /// </summary>
    private void InitGroundTransportToStoragesVariablesAndConstrains()
    {
        for (var i = 0; i < TransportsToStoragesCount; i++)
        {
            var transportToStoragesAssignsVector = new BoolVar[StoragesCount];
            for (var j = 0; j < StoragesCount; j++)
            {
                TransportsToStoragesAssignVariableMatrix[i, j] = Model.NewBoolVar($"transport_{i}_storage_{j}_assign");
                transportToStoragesAssignsVector[j] = TransportsToStoragesAssignVariableMatrix[i, j];
                var isGroundAssigned = Model.NewBoolVar("{i}_{j}_is_ground_delivery");
            }

            var transportToStoragesAssignsSum = LinearExpr.Sum(transportToStoragesAssignsVector);
            Model.Add(transportToStoragesAssignsSum <= 1);
        }
    }

    /// <summary>
    /// Инициализировать ограничение от транспорта до склада.
    /// </summary>
    private void InitGroundTransportsToStoragesAssignsConstrains()
    {
        for (var i = 0; i < StoragesCount; i++)
        {
            var transportsToStorageAssignsVector = new BoolVar[TransportsToStoragesCount];
            for (var j = 0; j < TransportsToStoragesCount; j++)
            {
                transportsToStorageAssignsVector[j] = TransportsToStoragesAssignVariableMatrix[j, i];
            }

            var transportsToStorageAssignsSum = LinearExpr.Sum(transportsToStorageAssignsVector);
            Model.Add(transportsToStorageAssignsSum <= 1);
            Model.Add(StorageAssignVariableVector[i] == transportsToStorageAssignsSum);
        }
    }
    
    /// <summary>
    /// Инициализировать переменные материалов.
    /// </summary>
    private void InitMaterialAssignVariables()
    {
        for (var i = 0; i < StoragesCount; i++)
        {
            for (var j = 0; j < MaterialsCount; j++)
            {
                MaterialInStorageAssignVariableMatrix[i, j] = Model.NewBoolVar($"storage_{i}_material_{j}_assign");
                if (MaterialParams[i, j] is null)
                {
                    Model.Add(MaterialInStorageAssignVariableMatrix[i, j] == 0);
                }
                else
                {
                    Model.Add(MaterialInStorageAssignVariableMatrix[i, j] <= 1)
                        .OnlyEnforceIf(StorageAssignVariableVector[i]);
                    Model.Add(MaterialInStorageAssignVariableMatrix[i, j] == 0)
                        .OnlyEnforceIf(StorageAssignVariableVector[i].Not());
                }
            }
        }
    }

    /// <summary>
    /// Инициализировать ограничения материалов.
    /// </summary>
    private void InitMaterialsConstraints()
    {
        for (var i = 0; i < MaterialsCount; i++)
        {
            var materialInStoragesAssignsVector = new BoolVar[StoragesCount];
            for (var j = 0; j < StoragesCount; j++)
            {
                materialInStoragesAssignsVector[j] = MaterialInStorageAssignVariableMatrix[j, i];
            }

            var materialInStoragesAssignsSum = LinearExpr.Sum(materialInStoragesAssignsVector);
            Model.Add(materialInStoragesAssignsSum == 1);
        }
    }

    /// <summary>
    /// Инициализировать ограничения и переменные от склада до НЕ наземного склада
    /// </summary>
    private void InitStoragesToNonGroundTransportsVariablesAndConstrains()
    {
        for (var i = 0; i < StoragesCount; i++)
        {
            var storageToNotGroundTransportsAssignsVector = new BoolVar[StoragesToNotGroundTransportsCount];
            for (var j = 0; j < StoragesToNotGroundTransportsCount; j++)
            {
                StoragesToNotGroundTransportsAssignVariableMatrix[i, j] =
                    Model.NewBoolVar($"storage_{i}_transport_fleet_{j}_assign");
                storageToNotGroundTransportsAssignsVector[j] = StoragesToNotGroundTransportsAssignVariableMatrix[i, j];
            }
            var storageToNotGroundTransportsAssignsSum = LinearExpr.Sum(storageToNotGroundTransportsAssignsVector);
            Model.Add(storageToNotGroundTransportsAssignsSum == StorageAssignVariableVector[i]).OnlyEnforceIf(IsGroundDeliveryVariable.Not());
            Model.Add(storageToNotGroundTransportsAssignsSum == 0).OnlyEnforceIf(IsGroundDeliveryVariable);
        }
    }
    
    /// <summary>
    /// Инициализировать ограничения от склада до НЕ наземного транспорта.
    /// </summary>
    private void InitStoragesToNonGroundTransportsConstrains()
    {
        for (var i = 0; i < StoragesToNotGroundTransportsCount; i++)
        {
            var storageToNotGroundTransportsAssignsVector = new BoolVar[StoragesCount];
            for (var j = 0; j < StoragesCount; j++)
            {
                storageToNotGroundTransportsAssignsVector[j] = StoragesToNotGroundTransportsAssignVariableMatrix[j, i];
            }
            var storageToNotGroundTransportsAssignsSum = LinearExpr.Sum(storageToNotGroundTransportsAssignsVector);
            Model.Add(storageToNotGroundTransportsAssignsSum == 0).OnlyEnforceIf(IsGroundDeliveryVariable);
            Model.Add(storageToNotGroundTransportsAssignsSum <= 1).OnlyEnforceIf(IsGroundDeliveryVariable.Not());
        }
    }

    public abstract void SetMinimization();
}