namespace WeatherAPI.Models;

public class City
{
	public string Name { get; }
	public string Country { get; }
	public short Temperature { get; }
	public string WeatherType { get; }
	public ushort Humidity { get; }
	public ushort Pressure { get; }
	public ushort Visibility { get; }

	public City(string name, string country, short temperature, string weatherType, ushort humidity, ushort pressure, ushort visibility)
	{
		Name = name;
		Country = country;
		Temperature = temperature;
		WeatherType = weatherType;
		Humidity = humidity;
		Pressure = pressure;
		Visibility = visibility;
	}
}
