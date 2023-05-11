using System.Net;

namespace SAPI
{
	public class Logger
	{
		private string dataFolder;
		private string logFile;

		public enum LogType
		{
			Access,
			System
		}

		private LogType logType;

		public Logger(string logName, LogType logType)
		{
			this.logType = logType;

			dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.ChangeExtension(Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().ProcessName), null));
			logFile = Path.Combine(dataFolder, logName + ".log");
			Console.WriteLine(logFile);
			Directory.CreateDirectory(dataFolder);
			File.Delete(logFile);
		}

		public void Log(string message = null, HttpListenerRequest request = null)
		{
			using (var sw = File.AppendText(logFile))
			{
				switch (logType)
				{
					case LogType.Access:
					{
						sw.WriteLine($"Time: {DateTime.Now}");
						sw.WriteLine($"URL: {request.Url}");
						sw.WriteLine($"Method: {request.HttpMethod}");
						sw.WriteLine($"User IPv4: {request.UserHostAddress}");
						sw.WriteLine($"User-Agent: {request.UserAgent}");
						sw.WriteLine("\n");
						break;
					}
					case LogType.System:
					{
						sw.WriteLine($"[ {DateTime.Now} ] {message}");
						break;
					}
				}
			}
		}
	}
}