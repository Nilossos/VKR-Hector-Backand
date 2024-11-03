using Backand.AlgorithmEntities;
using Backand.FrontendEntities.Requests;

namespace Backand.ManagersClasses.AlgorithmDataManager
{
	public class DataSorting
	{
		public static void SortTransportsOnFleetByFilter(List<TransportOnFleetWithRegions> transportsOnFleet, TargetMark filterMethod)
		{
			switch (filterMethod)
			{
				case TargetMark.Time:
					transportsOnFleet.Sort((t2, t1) => t1.TransportOnFleet.AverageSpeed.CompareTo(t2.TransportOnFleet.AverageSpeed));
					break;
				case TargetMark.Money:
					transportsOnFleet.Sort((t1, t2) => t1.TransportOnFleet.CoefficientValue.CompareTo(t2.TransportOnFleet.CoefficientValue));
					break;
				case TargetMark.Balanced:
					transportsOnFleet
						.Sort((t1, t2) => (t1.TransportOnFleet.CoefficientValue / t1.TransportOnFleet.AverageSpeed)
							.CompareTo(t2.TransportOnFleet.CoefficientValue / t2.TransportOnFleet.AverageSpeed));
					break;
			}
		}

		public static void SortCostAndTimeListByFilterMethod<T>(List<T> deliveryCosts, TargetMark filterMethod) where T : ICostAndTime
		{
			switch (filterMethod)
			{
				case TargetMark.Money:
					deliveryCosts.Sort((e1, e2) => e1.Cost.CompareTo(e2.Cost));
					break;
				case TargetMark.Time:
					deliveryCosts.Sort((e1, e2) => e1.DeliveryTime.CompareTo(e2.DeliveryTime));
					break;
				case TargetMark.Balanced:
					deliveryCosts.Sort((e1, e2) => (e1.Cost * e1.DeliveryTime).CompareTo(e1.Cost * e1.DeliveryTime));
					break;
				default:
					throw new NotImplementedException("Обработка такого значения фильтрации не была предусмотрена");
			};
		}
	}
}
