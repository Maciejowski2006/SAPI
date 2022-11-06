using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class Html : IEndpoint
{
	public string url { get; } = "html";
	public Method method { get; } = Method.GET;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		Utilities.HtmlResponse("Hello HTML", ref response);
	}
}
