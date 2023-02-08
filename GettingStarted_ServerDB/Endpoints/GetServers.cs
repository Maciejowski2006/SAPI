﻿using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities.Json;
using ServerAPI.Services;
using Server = ServerAPI.Models.Server;
namespace ServerAPI.Endpoints;

public class GetServers : IEndpoint
{
	public string url { get; } = "get-servers";
	public Method method { get; } = Method.GET;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		List<Server> servers = Database.GetServers();
		Json.Response(servers, ref response);
	}
}