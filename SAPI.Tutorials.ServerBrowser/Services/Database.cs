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

		private static List<string> apiKeys = new()
		{
			"1caa73f0-f076-490d-a1ee-18c33c057b66",
			"cb51a67b-afa1-4367-af3f-2db358a4e6d4",
			"c13dba40-7602-4676-943d-9b3ea1116255",
			"764fe931-53dd-40bb-b64b-f8a78381005e",
			"313cb56d-a596-4271-a8c1-f94cd253a86b",
		};
		public static List<string> GetApiKeys() => apiKeys;
	}
}