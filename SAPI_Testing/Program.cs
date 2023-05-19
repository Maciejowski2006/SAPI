using Testing.Endpoints;
using SAPI;

public class SAPI_Testing
{
	public static void Main()
	{
		Server sapi = new();
		
		sapi.Start();
	}
}