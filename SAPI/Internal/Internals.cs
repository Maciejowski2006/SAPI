using System.Net;
namespace SAPI;

internal static class Internals
{
	public static Logger access;
	public static Logger system;
	public static void PrintRequestInfo(HttpListenerRequest request)
	{
		access.Log(request: request);

		if (Config.ReadConfig().Verbose)
		{
			Console.WriteLine($"Time: {DateTime.Now}");
			Console.WriteLine($"URL: {request.Url}");
			Console.WriteLine($"Method: {request.HttpMethod}");
			Console.WriteLine($"User IPv4: {request.UserHostAddress}");
			Console.WriteLine($"User-Agent: {request.UserAgent}");
			Console.WriteLine("\n");
		}
	}
	public static void WriteLine(string message)
	{
		system.Log(message);
		Console.WriteLine(message);
	}
}
