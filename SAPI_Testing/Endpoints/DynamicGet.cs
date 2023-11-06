using System.Net;
using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints;

public class DynamicGet : Endpoint
{
    record Data(string id, string name);

	public override string url { get; } = "dynamic/:id/test/:name";
	protected override void Get(HttpListenerContext context, Dictionary<string, string> parameters)
	{
		Console.WriteLine(parameters["id"]);
		Console.WriteLine(parameters["name"]);

		Data data = new(parameters["id"], parameters["name"]);
		
		Json.Response(data, context);
	}
}
