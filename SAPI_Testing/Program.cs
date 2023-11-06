using SAPI;
using SAPI.API;
using SAPI.Auth;

public class SAPI_Testing
{
	public static void Main()
	{
		Server sapi = new();

		sapi.Use(new Auth());
		
		Debug.Log("Info");
		Debug.Warn("Warning");
		Debug.Error("Error");
		
		sapi.Start();
	}
}