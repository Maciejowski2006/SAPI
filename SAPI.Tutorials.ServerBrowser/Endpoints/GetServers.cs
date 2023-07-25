using SAPI;
using SAPI.API.Utilities;
using Tutorials.ServerBrowser.Services;

// ReSharper disable CheckNamespace
namespace Tutorials.ServerBrowser.Endpoints
{
	public class GetServers : Endpoint
	{
		public override string url { get; } = "get-servers";

		protected override void Get(ref Packet packet)
		{
			List<Models.Server> servers = Database.GetServers();
			Json.Response(servers, ref packet);
		}
	}
}