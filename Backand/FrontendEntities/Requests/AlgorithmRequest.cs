using System.Text.Json.Serialization;

namespace Backand.FrontendEntities.Requests
{
	public enum BuildType
	{
		NoMatter = 0,
		Placer,
		Block 
	}

	public enum TargetMark
	{
		Time = 0,
		Money,
		Balanced
	}

	public class Filter
	{
		[JsonPropertyName("targetMark")]
		public TargetMark TargetMark { get; init; }
		[JsonPropertyName("buildType")]
		public BuildType BuildType { get; init; }
		[JsonPropertyName("transportTypeIds")]
		public int[] TransportTypeIds { get; init; }
		[JsonPropertyName("certainLogists")]
		public CertainLogists CertainLogists { get; init; }
		[JsonPropertyName("certainManufacturers")]
		public CertainManufacturers CertainManufacturers { get; init; }
	}

	public class CertainLogists
	{
		public List<int> Ids { get; init; }
	}

	public class CertainManufacturers
	{
		public List<int> Ids { get; init; }
	}

	public class ConstructionWithFilters
	{
		public int ConstructionId { get; init; }
        //public Boolean Checked { get; init; }
        public Filter Filter { get; init; }
	}

	public class AlgorithmRequest
	{
		public List<ConstructionWithFilters> Data { get; init; }
	}
}
