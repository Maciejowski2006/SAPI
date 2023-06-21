using SAPI;
using SAPI.API.Utilities;
using WeatherAPI.Models;
using WeatherAPI.Services;

namespace WeatherAPI.Endpoints;

public class GetWeather : Endpoint
{

	public override string url { get; } = "get-weather/:country/:city";

	private List<City> cities;
	
	protected override void Get(ref Packet packet)
	{
		cities = Database.GetCities();

		foreach (City city in cities)
		{
			if (city.Country == packet.Paramters["country"] && city.Name == packet.Paramters["city"])
				Json.Response(city, ref packet);
		}
	}
}
