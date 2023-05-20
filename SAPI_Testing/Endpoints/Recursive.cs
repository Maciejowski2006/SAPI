using SAPI;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class Recursive : Endpoint
{
	public override string url { get; } = "recursive/{recursive}";
	protected override void Get(ref Packet packet)
	{
		string path = Path.Combine(Directory.GetCurrentDirectory(), "public");

		StaticContent.HostDirectoryRecursively(path, url, ref packet);
	}
}