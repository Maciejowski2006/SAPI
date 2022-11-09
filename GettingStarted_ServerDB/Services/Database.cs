using ServerAPI.Models;
namespace ServerAPI.Services;

public static class Database
{
	private static List<Server> servers = new();

	public static List<Server> GetServers() => servers;
	public static void AddServer(Server server) => servers.Add(server);
}
