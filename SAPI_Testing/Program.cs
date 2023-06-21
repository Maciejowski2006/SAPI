using Testing.Endpoints;
using SAPI;
using SAPI.API;

public class SAPI_Testing
{
	public static void Main()
	{
		Server sapi = new();
		
		Debug.Warn("Test");
		
		sapi.Start();
	}
}