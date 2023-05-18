using System.Net;
using SAPI;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class Recursive : IEndpoint
{
	public string url { get; } = "recursive/{recursive}";
	public void Get(ref Packet packet)
	{
		string path = Path.Combine(Directory.GetCurrentDirectory(), "public");

		StaticContent.HostDirectoryRecursively(path, url, ref packet);
	}
}