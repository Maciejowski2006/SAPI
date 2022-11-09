using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;
using WeatherAPI.Models;
using WeatherAPI.Services;


namespace WeatherAPI.Endpoints;

public class GetWeather : IEndpoint
{

	public string url { get; } = "get-weather/:country/:city";
	public Method method { get; } = Method.GET;

	private List<City> cities;
	
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		cities = Database.GetCities();

		foreach (City city in cities)
		{
			if (city.Country == parameters["country"] && city.Name == parameters["city"])
				Utilities.JsonResponse(city, ref response);
		}
	}
}
