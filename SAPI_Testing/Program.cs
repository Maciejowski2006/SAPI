using Testing.Endpoints;
using SAPI;

public class SAPI_Testing
{
	public static void Main()
	{
		Server sapi = new();

		sapi.MountEndpoint(new GetJson());
		sapi.MountEndpoint(new SendJson());
		sapi.MountEndpoint(new DynamicGet());
		sapi.MountEndpoint(new Html());
		sapi.MountEndpoint(new ApiAuth());
		sapi.MountEndpoint(new Static());
		sapi.MountEndpoint(new Dir());
		sapi.MountEndpoint(new Cookies());
		sapi.MountEndpoint(new Upload());
		sapi.MountEndpoint(new Recursive());
		
		sapi.Start();
	}
}