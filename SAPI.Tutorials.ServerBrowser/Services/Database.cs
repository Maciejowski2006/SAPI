using Tutorials.ServerBrowser.Models;

// ReSharper disable CheckNamespace
namespace Tutorials.ServerBrowser.Services
{
	public static class Database
	{
		private static List<Server> servers = new()
		{
			new Server("53.255.102.13", "Ty's Funhouse",  "Join if you are bored ;-)", "Tokyo", "Sandbox", 19, 32),
			new Server("32.78.99.255", "TRYHARDSONLY", "TRYHARDSONLY", "Prison", "Deathmatch", 4, 4),
			new Server("65.0.54.255", "I'm lonely", ":(", "Apartment", "Sandbox", 0, 32),
			new Server("85.26.0.255", "AVOCADO RP", "DRUGS | ALCOHOL | HEISTS | APPLY FOR WHITELIST HERE: https://youtu.be/d1YBv2mWll0", "[ WORKSHOP ] avocado_bigcity", "DarkRP", 74, 128),
			new Server("129.255.6.21", "[ OFFICIAL ] Achievement Hunters", "Looking to get all of the achievements? Join us!", "Neighbourhood", "Sandbox", 61, 64),
		};

		public static List<Server> GetServers() => servers;
		public static void AddServer(Server s) => servers.Add(s);
	}
}