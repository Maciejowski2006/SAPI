using SAPI;
using SAPI.Utilities;
using ServerAPI.Services;
using Server = ServerAPI.Models.Server;

namespace ServerAPI.Endpoints;

public class AddServer : Endpoint
{
	public override string url { get; } = "add-server";
	protected override void Post(ref Packet packet)
	{
		Json.Fetch(out Server server, ref packet);
		Database.AddServer(server);
	}
}
