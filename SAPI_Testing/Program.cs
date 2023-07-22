using Testing.Endpoints;
using SAPI;
using SAPI.API;

public class SAPI_Testing
{
	public static void Main()
	{
		Server sapi = new();
		
		Debug.Log("Info");
		Debug.Warn("Warning");
		Debug.Error("Error");
		
		sapi.Start();
	}
}