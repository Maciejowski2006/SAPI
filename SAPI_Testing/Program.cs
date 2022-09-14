using Testing.Endpoints;
using SAPI;
using SAPI.Endpoints;

public class SAPI_Testing
{
	public static void Main(string[] args)
	{
		Server sapi = new Server("http://localhost:8000/");
		
		sapi.MountEndpoint(new Get("test", Method.POST));
		sapi.MountEndpoint(new GetFromAPI("api", Method.GET));

		sapi.Start();
	}
}