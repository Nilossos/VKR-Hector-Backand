using Backand.AlgorithmEntities;
using Backand.FrontendEntities.Requests;
using System.Security.Cryptography.X509Certificates;

namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class AlgorithmResponse
	{
		public List<Order> Orders { get; init; } = new();
    }
}
