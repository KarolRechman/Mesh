using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Services;
using System.Net.Http;

namespace FunctionApp
{
	public class MeshFunction
	{
		private readonly IMeshService _meshService;

		public MeshFunction(IMeshService meshService)
		{
			_meshService = meshService;
		}

		[FunctionName("MeshFunction")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			try
			{
				var result = await _meshService.FindViewSpots(req);

				return new OkObjectResult(result);
			}
			catch (Exception e)
			{
				//var error = $"GetDrivers failed: {e.Message}";
				//log.LogError(error);
				//if (error.Contains(Constants.SECURITY_VALITION_ERROR))
				//	return new StatusCodeResult(401);
				//else
				return new BadRequestObjectResult(e.Message);
			}
		}
	}
}
