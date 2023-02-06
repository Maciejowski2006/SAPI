using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities.Auth;

namespace Testing.Endpoints;

public class ApiAuth : IEndpoint
{

	public string url { get; } = "auth";
	public Method method { get; } = Method.GET;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		List<BasicAuthCredentials> credentials = new ()
		{
			new BasicAuthCredentials("user", "pass"),
			new BasicAuthCredentials("other", "inny"),
			
		};
	
		List<string> keys = new()
		{
			"b4a4bc584acbd4",
			"asdf1",
			"bfasd5"
		};

		bool keyAuth = Auth.CheckForKey(keys, "x-api-key", ref request);
		bool userPassAuth = Auth.CheckForUserPass(credentials, ref request);
		
		Console.WriteLine($"Key Auth: {keyAuth}");
		Console.WriteLine($"User+Password Auth: {userPassAuth}");
	}
}
