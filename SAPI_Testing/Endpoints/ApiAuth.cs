using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints;

public class ApiAuth : Endpoint
{
	public override string url { get; } = "auth";

	protected override void Get(ref Packet packet)
	{
		List<BasicCredentials> credentials = new ()
		{
			new BasicCredentials("user", null, "pass"),
			new BasicCredentials("other", null,  "inny"),
		};

		List<string> keys = new()
		{
			"b4a4bc584acbd4",
			"asdf1",
			"bfasd5"
		};

		//bool keyAuth = Auth.GetApiKey(out string? key, "x-api-key", ref packet);

		// bool keyAuth = Auth.CheckForApiKey(keys, "x-api-key", ref packet);
		// bool userPassAuth = Auth.CheckForBasicCredentials(credentials, (pass) => pass, ref packet);

		// Console.WriteLine($"Key Auth: {keyAuth}");
		// Console.WriteLine($"User+Password Auth: {userPassAuth}");
	}
}
