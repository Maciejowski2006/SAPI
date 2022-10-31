using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class Simple : Endpoint
{

	public Simple(string url, Method method) : base(url, method) { }
	public override void Task(ref HttpListenerRequest request, ref HttpListenerResponse response)
	{
		string page = "Test of displaying page";

		Utilities.HtmlResponse(page, ref response);
	}
}
