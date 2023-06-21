using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints;

record DataModel(int id, string message); 
public class GetJson : Endpoint
{
	public override string url { get; } = "json";
	protected override void Get(ref Packet packet)
	{
		DataModel model = new(14, "Hello, World!");
		
		Json.Response(model, ref packet);
	}

	protected override void Post(ref Packet packet)
	{
		Json.Fetch(out DataModel model, ref packet);
		
		Console.WriteLine($"ID: {model.id}\nMessage: {model.message}");
		
		Json.Response(model, ref packet);
	}
}