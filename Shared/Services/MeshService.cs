using Microsoft.AspNetCore.Http;
using Models;
using Newtonsoft.Json;
using System.Text;
using Shared.Helpers;

namespace Shared.Services;

public class MeshService : IMeshService
{
	private readonly ViewSpotFinderHelper _viewSpotFinderHelper;

	public MeshService(ILoggerService loggerService)
	{
		_viewSpotFinderHelper = new ViewSpotFinderHelper(loggerService);
	}

	public async Task<List<ViewSpot>?> FindViewSpots(HttpRequest request)
	{
		FormParams formParams = await _viewSpotFinderHelper.RetrieveParams(request);

		if (formParams.Mesh.Values == null)
		{
			return null;
		}

		// Create a dictionary to map element IDs to their values
		Dictionary<int, double> elementValues = _viewSpotFinderHelper.PrepareElementValues(formParams.Mesh);

		// Create a dictionary to map nodes to the elements they belong to
		ILookup<int, int> nodeElements = _viewSpotFinderHelper.PrepareNodeElements(formParams.Mesh);

		// Create a priority queue to store elements sorted by value (highest first)
		var queue = _viewSpotFinderHelper.PrepareQueue(elementValues, formParams.Mesh.Elements);

		// Process elements in the queue until we have found n view spots or the queue is empty
		return GetViewSpotsFromQueue(formParams, queue, nodeElements, elementValues);
	}

	private static List<ViewSpot> GetViewSpotsFromQueue(FormParams formParams, SortedSet<int> queue, ILookup<int, int> nodeElements, Dictionary<int, double> elementValues)
	{
		var tolerance = Math.Abs(double.MinValue);
		var viewSpots = new List<ViewSpot>();

		Mesh mesh = formParams.Mesh;
		int numberOfSpots = formParams.NumberOfSpots;

		while (viewSpots.Count < numberOfSpots && queue.Count > 0)
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
			if (!isViewSpot) continue;
			{
				viewSpots.Add(new ViewSpot { ElementId = elementId, Value = elementValues[elementId] });

				// Remove neighboring elements from the queue if they have the same value
				foreach (var nodeId in mesh.Elements.First(e => e.Id == elementId).Nodes)
				{
					var neighborElements = nodeElements[nodeId].Where(ne => ne != elementId);
					foreach (var neighborElementId in neighborElements)
					{
						if (Math.Abs(elementValues[neighborElementId] - elementValues[elementId]) < tolerance)
						{
							queue.Remove(neighborElementId);
						}
					}
				}
			}
		}

		return viewSpots;
	}
}
