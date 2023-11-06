using System.Net;
using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints;

record DataModel(int id, string message);
public class GetJson : Endpoint
{
	public override string url { get; } = "json";
	protected override void Get(HttpListenerContext context, Dictionary<string, string> parameters)
	{
		DataModel model = new(14, "Hello, World!");
		
		Json.Response(model, context);
	}

	protected override void Post(HttpListenerContext context, Dictionary<string, string> parameters)
	{
		Json.Fetch(out DataModel model, context);
		
		Console.WriteLine($"ID: {model.id}\nMessage: {model.message}");
		
		Json.Response(model, context);
	}
}