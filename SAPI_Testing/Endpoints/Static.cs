using SAPI;
using SAPI.Utilities;

namespace Testing.Endpoints
{
	public class Static : IEndpoint
	{
		public string url { get; } = "static";
		public void Get(ref Packet packet)
		{
			string file = Path.Combine(Directory.GetCurrentDirectory(), "linus.mov");
			
			StaticContent.FileResponse(file, ref packet);
		}
	}
}
