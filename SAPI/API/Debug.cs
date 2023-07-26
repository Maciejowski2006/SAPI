
namespace SAPI.API
{
	public enum LogSeverity
	{
		Info,
		Warn,
		Error
	}
	
	public static class Debug
	{
		public static void Log(string message) => LLAPI.Debug.LogImpl(message, LogSeverity.Info);
		public static void Warn(string message) => LLAPI.Debug.LogImpl(message, LogSeverity.Warn);
		public static void Error(string message) => LLAPI.Debug.LogImpl(message, LogSeverity.Error);
	}
}
