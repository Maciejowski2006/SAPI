using System.Net;
using SAPI.API;

namespace SAPI.LLAPI
{
	internal static class Debug
	{
		private static string dataFolder;
		private static string systemLog;
		private static string accessLog;
		private static string userLog;

		public static void Init()
		{
			dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.ChangeExtension(Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().ProcessName), null));
			systemLog = Path.Combine(dataFolder, "system.log");
			accessLog = Path.Combine(dataFolder, "access.log");
			userLog = Path.Combine(dataFolder, "user.log");
			Directory.CreateDirectory(dataFolder);
			File.Delete(systemLog);
			File.Delete(accessLog);
			File.Delete(userLog);
		}

		internal static void Log(HttpListenerRequest request)
		{
			using var sw = File.AppendText(accessLog);
			
			sw.WriteLine($"Time: {DateTime.Now}");
			sw.WriteLine($"URL: {request.Url}");
			sw.WriteLine($"Method: {request.HttpMethod}");
			sw.WriteLine($"User IPv4: {request.UserHostAddress}");
			sw.WriteLine($"User-Agent: {request.UserAgent}");
			sw.WriteLine("\n");
		}

		internal static void Log(string message) => LogImpl(message, LogSeverity.Info, systemLog);
		internal static void Warn(string message) => LogImpl(message, LogSeverity.Warn, systemLog);
		internal static void Error(string message) => LogImpl(message, LogSeverity.Error, systemLog);

		internal static void LogImpl(string message, LogSeverity severity, string? logFile = null)
		{
			Dictionary<LogSeverity, ConsoleColor> logColor = new()
			{
				{ LogSeverity.Info, ConsoleColor.White },
				{ LogSeverity.Warn, ConsoleColor.Yellow },
				{ LogSeverity.Error, ConsoleColor.Red },
			};

			Console.ForegroundColor = logColor[severity];
			Console.WriteLine($"[ {DateTime.Now} | {severity} ] {message}");
			Console.ResetColor();
			
			using StreamWriter sw = File.AppendText(logFile ?? userLog);
			sw.WriteLine($"[ {DateTime.Now} ] {message}");
		}
	}
}