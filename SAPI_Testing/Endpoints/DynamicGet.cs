using SAPI;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class DynamicGet : IEndpoint
{
    record Data(string id, string name);

	public string url { get; } = "dynamic/:id/test/:name";
	public void Get(ref Packet packet)
	{
		Console.WriteLine(packet.Paramters["id"]);
		Console.WriteLine(packet.Paramters["name"]);

		Data data = new(packet.Paramters["id"], packet.Paramters["name"]);
		
		Json.Response(data, ref packet);
	}
}
