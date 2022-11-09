using SAPI;
using WeatherAPI.Endpoints;

namespace ServerAPI;

public class ServerAPI
{
	public static void Main()
	{
		Server sapi = new();

		sapi.MountEndpoint(new GetWeather());
		
		sapi.Start();
	}
}
