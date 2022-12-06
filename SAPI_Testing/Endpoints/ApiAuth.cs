using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class ApiAuth : IEndpoint
{

	public string url { get; } = "auth";
	public Method method { get; } = Method.GET;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		List<BasicAuthCredentials> credentials = new ()
		{
			new BasicAuthCredentials("dub", "iel"),
			new BasicAuthCredentials("user", "pass"),
			new BasicAuthCredentials("user", "inny"),
			
		};
		bool keyAuth = Utilities.CheckForKeyAuthorization(new List<string>() {"api", "bruh", "duh"}, "x-api-key", ref request);
		bool userPassAuth = Utilities.CheckForUserPassAuthorization(credentials, ref request);
		
		Console.WriteLine($"Key Auth: {keyAuth}");
		Console.WriteLine($"User+Password Auth: {userPassAuth}");
	}
}
