using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;
namespace Testing.Endpoints;

public class DynamicGet : IEndpoint
{
    record Data(string id, string name);

	public string url { get; } = "dynamic/:id/test/:name";
	public Method method { get; } = Method.GET;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		Console.WriteLine(parameters["id"]);
		Console.WriteLine(parameters["name"]);

		Data data = new(parameters["id"], parameters["name"]);
		
		Utilities.JsonResponse(data, ref response);
	}
}
