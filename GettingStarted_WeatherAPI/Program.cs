using SAPI;
using WeatherAPI.Endpoints;
using WeatherAPI.Services;

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
