using Microsoft.AspNetCore.Http;
using Models;
using Newtonsoft.Json;
using System.Text;

namespace Shared.Services;

public class MeshService : IMeshService
{
	private ILoggerService _loggerService;

	public MeshService(ILoggerService loggerService)
	{
		_loggerService = loggerService;
	}

	public async Task<List<ViewSpot>> FindViewSpots(HttpRequest request)
	{
		var formParams = await RetrieveParams(request);
		var mesh = formParams.Mesh;
		var viewSpots = new List<ViewSpot>();

		// Create a dictionary to map element IDs to their values
		var elementValues = mesh.Values.ToDictionary(v => v.ElementId, v => v.Value);

		// Create a dictionary to map nodes to the elements they belong to
		var nodeElements = mesh.Elements.SelectMany(e => e.Nodes.Select(n => new { NodeId = n, ElementId = e.Id }))
			.ToLookup(ne => ne.NodeId, ne => ne.ElementId);

		// Create a priority queue to store elements sorted by value (highest first)
		var queue = new SortedSet<int>(Comparer<int>.Create((x, y) => elementValues[y].CompareTo(elementValues[x])));

		// Add all elements to the queue
		foreach (var element in mesh.Elements)
		{
			queue.Add(element.Id);
		}

		// Process elements in the queue until we have found n view spots or the queue is empty
		while (viewSpots.Count < formParams.NumberOfSpots && queue.Count > 0)
		{
			// Get the element with the highest value from the queue
			var elementId = queue.First();
			queue.Remove(elementId);

			// Check if this is a view spot (i.e. all neighboring elements have lower values)
			var isViewSpot = true;
			foreach (var nodeId in mesh.Elements.First(e => e.Id == elementId).Nodes)
			{
				var neighborElements = nodeElements[nodeId].Where(ne => ne != elementId);
				foreach (var neighborElementId in neighborElements)
				{
					if (elementValues[neighborElementId] >= elementValues[elementId])
					{
						isViewSpot = false;
						break;
					}
				}
				if (!isViewSpot)
				{
					break;
				}
			}

			// Add the element to the list of view spots if it is a view spot
			if (isViewSpot)
			{
				viewSpots.Add(new ViewSpot { ElementId = elementId, Value = elementValues[elementId] });

				// Remove neighboring elements from the queue if they have the same value
				foreach (var nodeId in mesh.Elements.First(e => e.Id == elementId).Nodes)
				{
					var neighborElements = nodeElements[nodeId].Where(ne => ne != elementId);
					foreach (var neighborElementId in neighborElements)
					{
						if (elementValues[neighborElementId] == elementValues[elementId])
						{
							queue.Remove(neighborElementId);
						}
					}
				}
			}
		}

		return viewSpots;
	}

	public async Task<FormParams> RetrieveParams(HttpRequest request)
	{
		try
		{
			IFormCollection? formData = await request.ReadFormAsync();
			IFormFile file = formData?.Files["mesh"] ?? throw new InvalidOperationException();
			var numberOfSpots = int.Parse(request.Form["numberOfSpots"]);

			return new FormParams()
			{
				Mesh = DeserializeJsonFromFile(file),
				NumberOfSpots = numberOfSpots,
			};
		}
		catch (Exception e)
		{
			_loggerService.Log(e.Message);
			throw;
		}
	}

	private Mesh DeserializeJsonFromFile(IFormFile file)
	{
		using (var stream = new MemoryStream())
		{
			try
			{
				file.CopyTo(stream);
				byte[] data = stream.ToArray();

				var mesh = JsonConvert.DeserializeObject<Mesh>(Encoding.UTF8.GetString(data));

				return mesh;
			}
			catch (Exception e)
			{
				_loggerService.Log(e.Message);
				throw;
			}
		}
	}
}
