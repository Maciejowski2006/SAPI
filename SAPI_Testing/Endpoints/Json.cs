using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

internal record JsonModel(string name, int age);
public class Json : IEndpoint
{
	public string url { get; set; } = "json";
	public Method method { get; set; } = Method.GET;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response)
	{
		JsonModel model = new("test", 69);

		Utilities.JsonResponse(model, ref response);
	}
}
