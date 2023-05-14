using Microsoft.AspNetCore.Http;
using Models;
using Newtonsoft.Json;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
	public class ViewSpotFinderHelper
	{
		private readonly ILoggerService _loggerService;
		public ViewSpotFinderHelper(ILoggerService loggerService)
		{
			_loggerService = loggerService;
		}

		public SortedSet<int> PrepareQueue(Dictionary<int, double> elementValues, List<Element> meshElements)
		{
			var queue = new SortedSet<int>(Comparer<int>.Create((x, y) => elementValues[y].CompareTo(elementValues[x])));

			// Add all elements to the queue
			foreach (var element in meshElements)
			{
				queue.Add(element.Id);
			}

			return queue;
		}

		public ILookup<int, int> PrepareNodeElements(Mesh mesh)
		{
			return mesh.Elements.SelectMany(e => e.Nodes.Select(n => new { NodeId = n, ElementId = e.Id }))
				.ToLookup(ne => ne.NodeId, ne => ne.ElementId);
		}

		public Dictionary<int, double> PrepareElementValues(Mesh mesh)
		{
			return mesh.Values!.ToDictionary(v => v.ElementId, v => v.Value);
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
}
