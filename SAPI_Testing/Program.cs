using Testing.Endpoints;
using SAPI;
using SAPI.Endpoints;

public class SAPI_Testing
{
	public static void Main(string[] args)
	{
		Server sapi = new();
		
		sapi.MountEndpoint(new Get("test", Method.POST));
		sapi.MountEndpoint(new GetFromAPI("api", Method.GET));
		sapi.MountEndpoint(new Forbid("forbid", Method.GET));
		sapi.MountEndpoint(new Simple("simple", Method.GET));
		sapi.MountIEndpoint(new FromInterface());

		sapi.Start();
	}
}