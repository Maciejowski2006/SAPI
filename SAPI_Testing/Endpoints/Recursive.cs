using System.Net;
using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints;

public class Recursive : Endpoint
{
	public override string url { get; } = "recursive/{recursive}";
	protected override void Get(HttpListenerContext context, Dictionary<string, string> parameters)
	{
		string path = Path.Combine(Directory.GetCurrentDirectory(), "public");

		// StaticContent.HostDirectoryRecursively(path, url, ref packet);
		FileIO.ServeDirectoryRecursively(path, url, context);
	}
}