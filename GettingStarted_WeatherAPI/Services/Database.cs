﻿using WeatherAPI.Models;

namespace WeatherAPI.Services;

public static class Database
{
	private static List<City> cities = new();

	public static List<City> GetCities() => cities;

	public static void PopulateDB()
	{
		cities.Add(new City("Sydney", "Australia", 22, "Clouds and sun", 52, 1021, 16));
		cities.Add(new City("Vancouver", "Canada", 4, "Light Rain", 59, 1007, 13));
		cities.Add(new City("New_Delhi", "India", 21, "Clear", 74, 1014, 10));
		cities.Add(new City("Tokyo", "Japan", 14, "Sunny", 67, 1018, 18));
		cities.Add(new City("Krakow", "Poland", 3, "Fog", 100, 1019, 8));
		cities.Add(new City("Pretoria", "South_Africa", 14, "Cloudy", 90, 1016, 5));
		cities.Add(new City("New_York", "USA", 22, "Clear", 15, 1023, 16));
	}
}
