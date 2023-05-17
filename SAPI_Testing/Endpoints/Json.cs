using System.Net;
using SAPI;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

record DataModel(int id, string message); 
public class GetJson : IEndpoint
{
	public string url { get; } = "get-json";
	public Method method { get; } = Method.GET;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		DataModel model = new(14, "Hello, World!");
		
		Json.Response(model, ref response);
	}
}
public class SendJson : IEndpoint
{
	public string url { get; } = "send-json";
	public Method method { get; } = Method.POST;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		Json.Fetch(out DataModel model, ref request);
		
		Console.WriteLine($"ID: {model.id}\nMessage: {model.message}");
		
		Json.Response(model, ref response);
	}
}