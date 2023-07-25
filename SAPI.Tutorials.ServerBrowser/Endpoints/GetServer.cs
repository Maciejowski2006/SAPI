using SAPI;
using SAPI.API.Utilities;
using Tutorials.ServerBrowser.Services;

namespace Tutorials.ServerBrowser.Endpoints
{
	public class GetServer : Endpoint
	{
		public override string url { get; } = "get-server/:ip";

		protected override void Get(ref Packet packet)
		{
			List<Models.Server> db = Database.GetServers();

			string serverIp = packet.Paramters["ip"];
			Models.Server? server = db.Find(x => x.IpAddress == serverIp);

			Json.Response(server, ref packet);
		}
	}
}