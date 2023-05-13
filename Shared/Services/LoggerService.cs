namespace Shared.Services;

public class LoggerService : ILoggerService
{
	private readonly string _logFilePath;

	public LoggerService(ISettingService settingService)
	{
		_logFilePath = settingService.GetLogFilePath();
	}

	public void Log(string message)
	{
		var logMessage = $"{DateTime.Now}: {message}";
		File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
	}
}