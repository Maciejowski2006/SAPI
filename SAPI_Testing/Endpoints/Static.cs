using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints
{
	public class Static : Endpoint
	{
		public override string url { get; } = "static";
		protected override void Get(ref Packet packet)
		{
			string file = Path.Combine(Directory.GetCurrentDirectory(), "linus.mov");
			
			// StaticContent.FileResponse(file, ref packet);
			FileIO.ServeFile(file, ref packet);
		}
	}
}
