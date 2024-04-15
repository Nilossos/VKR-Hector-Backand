using Backand.AlgorithmEntities;
using Backand.Services.AlgorithmServices.AlgorithmPreparers.Values;

namespace Backand.Services.AlgorithmServices.AlgorithmPreparers;

public static class AlgorithmDataTransformerExtensions
{
    public static long[] TransformDecimalToLongVector(this IEnumerable<decimal> vector, int factor)
    {
        return vector.Select(value => (long)(value * factor)).ToArray();
    }
    
    public static long[] TransformFloatToLongVector(this IEnumerable<float> vector, int factor)
    {
        return vector.Select(value => (long)(value * factor)).ToArray();
    }

    public static long[,] TransformDecimalToLongMatrix(this decimal[,] matrix, int factor)
    {
        var xDimensionLength = matrix.GetLength(0);
        var yDimensionLength = matrix.GetLength(1);
        var result = new long[xDimensionLength, yDimensionLength];
        for (var i = 0; i < xDimensionLength; i++)
        {
            for (var j = 0; j < yDimensionLength; j++)
            {
                result[i, j] = (long)(matrix[i, j] * factor);
            }
        }
        return result;
    }

    public static LongMaterialParams?[,] TransformMaterialParamsToLongMatrix(this MaterialParams?[,] matrix, int factor)
    {
        var storagesCount = matrix.GetLength(0);
        var materialsCount = matrix.GetLength(1);
        var result = new LongMaterialParams?[storagesCount, materialsCount];
        for (var i = 0; i < storagesCount; i++)
        {
            for (var j = 0; j < materialsCount; j++)
            {
                if (matrix[i, j] == null)
                {
                    result[i, j] = null;
                    continue;
                }
                result[i, j] = new LongMaterialParams
                {
                    Amount = (long)matrix[i, j]!.Amount,
                    PricePerUnit = (long)(matrix[i, j]!.PricePerUnit * factor)
                };
            }
        }
        return result;
    }
    
    public static LongTransportInfo[] TransformTransportInfosToLongMatrix(this IEnumerable<TransportInfo> transportInfos, int factor)
    {
        return transportInfos.Select(transportInfo =>
            new LongTransportInfo
            {
                AverageSpeed = transportInfo.AverageSpeed,
                Coefficient = (long)(transportInfo.Coefficient * factor)
            })
            .ToArray();
    }
}