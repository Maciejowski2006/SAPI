using System.Net;
using SAPI.Endpoints;

namespace Testing.Endpoints;

public class Get : Endpoint
{
	public override void Task(ref HttpListenerRequest request, ref HttpListenerResponse response)
	{
		Console.WriteLine("GETGETGET");
	}
	public Get(string url, Method method) : base(url, method) { }
}
