using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;
using ServerAPI.Models;
using ServerAPI.Services;

namespace ServerAPI.Endpoints;

public class AddServer : IEndpoint
{
	public string url { get; } = "add-server";
	public Method method { get; } = Method.POST;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		Json.Fetch(out Server server, ref request);
		Database.AddServer(server);
	}
}
