using Backand.AlgorithmEntities;
using Backand.FrontendEntities.Requests;

namespace Backand.ManagersClasses.AlgorithmDataManager
{
	public class DataSorting
	{
		public static void SortTransportsOnFleetByFilter(List<TransportOnFleetWithRegions> transportsOnFleet, FilterMethod filterMethod)
		{
			switch (filterMethod)
			{
				case FilterMethod.Time:
					transportsOnFleet.Sort((t2, t1) => t1.TransportOnFleet.AverageSpeed.CompareTo(t2.TransportOnFleet.AverageSpeed));
					break;
				case FilterMethod.Money:
					transportsOnFleet.Sort((t1, t2) => t1.TransportOnFleet.CoefficientValue.CompareTo(t2.TransportOnFleet.CoefficientValue));
					break;
				case FilterMethod.Balanced:
					transportsOnFleet
						.Sort((t1, t2) => (t1.TransportOnFleet.CoefficientValue / t1.TransportOnFleet.AverageSpeed)
							.CompareTo(t2.TransportOnFleet.CoefficientValue / t2.TransportOnFleet.AverageSpeed));
					break;
			}
		}

		public static void SortCostAndTimeListByFilterMethod<T>(List<T> deliveryCosts, FilterMethod filterMethod) where T : ICostAndTime
		{
			switch (filterMethod)
			{
				case FilterMethod.Money:
					deliveryCosts.Sort((e1, e2) => e1.Cost.CompareTo(e2.Cost));
					break;
				case FilterMethod.Time:
					deliveryCosts.Sort((e1, e2) => e1.DeliveryTime.CompareTo(e2.DeliveryTime));
					break;
				case FilterMethod.Balanced:
					deliveryCosts.Sort((e1, e2) => (e1.Cost * e1.DeliveryTime).CompareTo(e1.Cost * e1.DeliveryTime));
					break;
				default:
					throw new NotImplementedException("Обработка такого значения фильтрации не была предусмотрена");
			};
		}
	}
}
