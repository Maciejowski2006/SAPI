using SAPI;
using SAPI.API.Utilities;
using ServerAPI.Services;
using Server = ServerAPI.Models.Server;
namespace ServerAPI.Endpoints;

public class GetServers : Endpoint
{
	public override string url { get; } = "get-servers";
	protected override void Get(ref Packet packet)
	{
		List<Server> servers = Database.GetServers();
		Json.Response(servers, ref packet);
	}
}