namespace Models;

public class Mesh
{
	public List<Node> Nodes { get; set; }
	public List<Element> Elements { get; set; }
	public List<Values>? Values { get; set; }
}