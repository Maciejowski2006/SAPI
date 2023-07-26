using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints;

public class DynamicGet : Endpoint
{
    record Data(string id, string name);

	public override string url { get; } = "dynamic/:id/test/:name";
	protected override void Get(ref Packet packet)
	{
		Console.WriteLine(packet.Parameters["id"]);
		Console.WriteLine(packet.Parameters["name"]);

		Data data = new(packet.Parameters["id"], packet.Parameters["name"]);
		
		Json.Response(data, ref packet);
	}
}
