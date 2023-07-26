using System.Net;

namespace SAPI.LLAPI
{
	internal static class Internals
	{
		public static void PrintRequestInfo(HttpListenerRequest request)
		{
			Debug.Log(request);

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
	}
}