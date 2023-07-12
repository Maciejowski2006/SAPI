using SAPI;
using SAPI.API.Utilities;
using Tutorials.ServerBrowser.Services;

// ReSharper disable CheckNamespace
namespace Tutorials.ServerBrowser.Endpoints
{
	public class AddServer : Endpoint
	{
		public override string url { get; } = "add-server";

		protected override void Post(ref Packet packet)
		{
			List<string> apiKeys = Database.GetApiKeys();
			if (Auth.CheckForApiKey(apiKeys, ref packet))
			{
				Json.Fetch(out Models.Server server, ref packet);
				Database.AddServer(server);
			}
			else
			{
				Error.ErrorPageResponse(HttpStatus.Unauthorized, ref packet);
			}
		}
	}
}