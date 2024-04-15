using Backand.AlgorithmEntities;

namespace Backand.Services.AlgorithmServices.AlgorithmPreparers.Values;

public class LongMaterialParams
{
    public long PricePerUnit { get; set; }
    public long Amount { get; set; }
    public long Cost => PricePerUnit * Amount;
}