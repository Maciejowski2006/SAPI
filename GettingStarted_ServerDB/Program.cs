using SAPI;
using ServerAPI.Endpoints;

namespace ServerAPI;

public class ServerAPI
{
	public static void Main()
	{
		Server sapi = new();
		
		sapi.Start();
	}
}