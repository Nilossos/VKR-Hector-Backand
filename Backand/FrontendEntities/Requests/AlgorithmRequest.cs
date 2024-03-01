using System.Text.Json.Serialization;

namespace Backand.FrontendEntities.Requests
{
	public enum BuildType
	{
		NoMatter = 0,
		Placer,
		Block
	}

	public enum FilterMethod
	{
		Time = 0,
		Money,
		Balanced
	}

	public class Filter
	{
		[JsonPropertyName("filterMethod")]
		public FilterMethod FilterMethod { get; init; }
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

	public class ConstructionOption
	{
		public int ConstructionId { get; init; }
		public Filter Filter { get; init; }
	}



	public class AlgorithmRequest
	{
		public List<ConstructionOption> ConstructionOptions { get; init; }

		/*
		примерный запрос с фронта:
		"фильтры"=[
			{
				"constructionId":number,
				"filters":{
					"base":"time"|"money"|"balans",
					"transportTypeIds":[],
					"buildType":1|2|0//0-пофиг, 1-россыпь, 2-блочный способ, ...
					"certainLogists": [
						"лог_компания1",
						"лог_компания2"...
					]
					"certain_manufacturers": [
						"производитель1",
						"производитель2"...
					]
				}
			}
		]
		*/
	}
}
