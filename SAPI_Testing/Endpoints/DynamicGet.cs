using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;
namespace Testing.Endpoints;

public class DynamicGet : IEndpoint
{

	public string url { get; } = "dynamic/:id/this/:err";
	public Method method { get; } = Method.GET;
	public List<string> parameters { get; set; } = new();
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, List<string> parameters)
	{
		Console.WriteLine(parameters[0]);
		Console.WriteLine(parameters[1]);
		Utilities.Error(HttpStatus.OK, ref response);
	}
}
