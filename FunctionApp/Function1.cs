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

namespace FunctionApp
{
	public static class Function1
	{
		[FunctionName("Function1")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			try
			{
				var meshService = ServiceFactory.GetMeshService();

				return new OkObjectResult(meshService);
			}
			catch (Exception e)
			{
				var error = $"GetDrivers failed: {e.Message}";
				log.LogError(error);
				if (error.Contains(Constants.SECURITY_VALITION_ERROR))
					return new StatusCodeResult(401);
				else
					return new BadRequestObjectResult(error);
			}
		}
	}
}
