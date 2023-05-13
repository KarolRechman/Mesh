using System.Text.Json.Serialization;

namespace Models;

public class Element
{
	public int Id { get; set; }
	public List<int> Nodes { get; set; }
}