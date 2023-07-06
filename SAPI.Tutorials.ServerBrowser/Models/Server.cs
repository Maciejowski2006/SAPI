// ReSharper disable CheckNamespace
namespace Tutorials.ServerBrowser.Models
{
	public class Server
	{
		public string IpAddress { get; }
		public string Name { get; }
		public string Description { get; }
		public string Map { get; }
		public string Gamemode { get; }
		public ushort PlayerCount { get; }
		public ushort MaxPlayers { get; }
		
		public Server(string ipAddress, string name, string description, string map, string gamemode, ushort playerCount, ushort maxPlayers)
		{
			IpAddress = ipAddress;
			Name = name;
			Description = description;
			Map = map;
			Gamemode = gamemode;
			PlayerCount = playerCount;
			MaxPlayers = maxPlayers;
		}
	}
}