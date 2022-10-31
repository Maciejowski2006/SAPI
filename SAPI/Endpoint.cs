using System.Net;

namespace SAPI.Endpoints;

public abstract class Endpoint
{
	public string url;
	public Method method;

	public Endpoint(string url, Method method)
	{
		this.url = url;
		this.method = method;
	}

	/// <summary>
	/// In task you execute code to satisfy the request
	/// </summary>
	/// <param name="request">Request info from server</param>
	/// <param name="response">Response is used to communicate to client</param>
	public abstract void Task(ref HttpListenerRequest request, ref HttpListenerResponse response);
}
