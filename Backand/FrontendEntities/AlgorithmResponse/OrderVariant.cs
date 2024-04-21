namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class OrderVariant
	{
		public IEnumerable<MaterialOrderVariant> MaterialOrderVariants { get; set; }

		public OrderResult OrderResult
		{
			get
			{
				var materialsCost = MaterialOrderVariants.Sum(variant => variant.ProductionInfo.PurchasePrice);
				var timeByTransport = new Dictionary<(Guid trackId, string transportTypeName), decimal>();
				var costByTransport = new Dictionary<(Guid trackId, string transportTypeName), decimal>();
				var logisticInfos = MaterialOrderVariants.SelectMany(variant => variant.LogisticInfos);
				foreach (var logisticInfo in logisticInfos)
				{
					var key = (logisticInfo.TrackId, logisticInfo.TransportTypeName);
					timeByTransport.TryAdd(key,
						logisticInfo.DeliveryTime);
					costByTransport.TryAdd(key,
						logisticInfo.DeliveryCost);
				}
				return new OrderResult(materialsCost + costByTransport.Sum(a => a.Value),
					timeByTransport.Sum(a => a.Value));
			}
		}

		public OrderVariant(IEnumerable<MaterialOrderVariant> materialOrderVariants, OrderResult orderResult)
		{
			MaterialOrderVariants = materialOrderVariants;
			//OrderResult = orderResult;
		}

		public OrderVariant() { }
		
		public bool IsAssemblyBuildRequired { get; set; }
	}
}
