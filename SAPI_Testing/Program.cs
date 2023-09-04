using SAPI;
using Debug = SAPI.API.Debug;

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