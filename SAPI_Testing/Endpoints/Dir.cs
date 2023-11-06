using System.Net;
using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints
{
	public class Dir : Endpoint
	{
		public override string url { get; } = "dir/:file";
		protected override void Get(HttpListenerContext context, Dictionary<string, string> parameters)
		{
			string path = Path.Combine(Directory.GetCurrentDirectory(), "public");

			FileIO.ServeDirectory(path, context, parameters);
		}
	}
}
