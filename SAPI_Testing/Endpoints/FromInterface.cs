using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class FromInterface : IEndpoint
{
	public string url { get; set; } = "interface";
	public Method method { get; set; } = Method.GET;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response)
	{
		Utilities.HtmlResponse("hi", ref response);
	}
}

}

