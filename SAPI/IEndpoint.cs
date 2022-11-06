using System.Net;

namespace SAPI.Endpoints;

public enum Method
{
	GET,
	POST,
	PUT,
	PATCH,
	DELETE,
	OPTIONS,
	HEAD
}
public interface IEndpoint
{
	string url { get; }
	Method method { get; }

	/// <summary>
	/// In task you execute code to satisfy the request
	/// </summary>
	/// <param name="request">Request info from server</param>
	/// <param name="response">Response is used to communicate to client</param>
	/// <param name="parameters">List of parameters provided from dynamic endpoint</param>
	void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters);
}
