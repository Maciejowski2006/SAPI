using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints
{
	public class Dir : Endpoint
	{
		public override string url { get; } = "dir/:file";
		protected override void Get(ref Packet packet)
		{
			string path = Path.Combine(Directory.GetCurrentDirectory(), "public");

			// StaticContent.HostDirectory(path, ref packet);
			FileIO.ServeDirectory(path, ref packet);
		}
		
	}
}
