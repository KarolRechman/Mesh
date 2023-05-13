using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using Shared.Services;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddHttpClient();

			builder.Services.AddTransient<IMeshService, MeshService>();
			builder.Services.AddTransient<ISettingService, SettingService>();
			builder.Services.AddTransient<ILoggerService, LoggerService>();
		}
	}
}
