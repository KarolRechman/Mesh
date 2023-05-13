using System;

namespace Shared.Services;

public class SettingService : ISettingService
{
	private const string LogFileName = "LogsFunctionApp.txt";

	public string GetLogFilePath()
	{
		var appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

		return appDirectory == null ? throw new Exception("Unable to determine the application directory.") : Path.Combine(appDirectory, LogFileName);
	}

}