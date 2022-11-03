using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;
namespace Testing.Endpoints;

public class Html : IEndpoint
{

	public string url { get; } = "html";
	public Method method { get; }
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		Utilities.HtmlResponse("html get request", ref response);
	}
}
