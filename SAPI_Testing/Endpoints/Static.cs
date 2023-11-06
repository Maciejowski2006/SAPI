using System.Net;
using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints
{
	public class Static : Endpoint
	{
		public override string url { get; } = "static";
		protected override void Get(HttpListenerContext context, Dictionary<string, string> parameters)
		{
			string file = Path.Combine(Directory.GetCurrentDirectory(), "linus.mov");
			
			FileIO.ServeFile(file, context);
		}
	}
}
