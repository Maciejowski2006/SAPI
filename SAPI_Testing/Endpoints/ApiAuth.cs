using System.Net;
using SAPI;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class ApiAuth : IEndpoint
{
	public string url { get; } = "auth";

	public void Get(ref Packet packet)
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

		bool keyAuth = Auth.CheckForKey(keys, "x-api-key", ref packet);
		bool userPassAuth = Auth.CheckForUserPass(credentials, ref packet);
		
		Console.WriteLine($"Key Auth: {keyAuth}");
		Console.WriteLine($"User+Password Auth: {userPassAuth}");
	}
}
