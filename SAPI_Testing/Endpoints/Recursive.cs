using System.Net;
using SAPI;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class Recursive : IEndpoint
{
	public string url { get; } = "recursive/{recursive}";
	public Method method { get; } = Method.GET;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		string path = Path.Combine(Directory.GetCurrentDirectory(), "public");

		StaticContent.HostDirectoryRecursively(path, url, ref request, ref response);
	}
}