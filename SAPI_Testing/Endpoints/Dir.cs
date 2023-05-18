using SAPI;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints
{
	public class Dir : IEndpoint
	{
		public string url { get; } = "dir/:file";
		public void Get(ref Packet packet)
		{
			string path = Path.Combine(Directory.GetCurrentDirectory(), "public");

			StaticContent.HostDirectory(path, ref packet);
		}
		
	}
}
