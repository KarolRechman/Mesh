using Newtonsoft.Json;

namespace Models;

public class Values
{
	[JsonProperty("element_id")]
	public int ElementId { get; set; }
	public double Value { get; set; }
}