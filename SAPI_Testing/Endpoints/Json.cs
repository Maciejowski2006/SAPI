using SAPI;
using SAPI.Utilities;

namespace Testing.Endpoints;

record DataModel(int id, string message); 
public class GetJson : IEndpoint
{
	public string url { get; } = "json";
	public void Get(ref Packet packet)
	{
		DataModel model = new(14, "Hello, World!");
		
		Json.Response(model, ref packet);
	}

	public void Post(ref Packet packet)
	{
		Json.Fetch(out DataModel model, ref packet);
		
		Console.WriteLine($"ID: {model.id}\nMessage: {model.message}");
		
		Json.Response(model, ref packet);
	}
}