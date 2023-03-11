using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities.StaticContent;

namespace Testing.Endpoints
{
	public class Dir : IEndpoint
	{
		public string url { get; } = "dir/:file";
		public Method method { get; }
		public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
		{
			string path = Path.Combine(Directory.GetCurrentDirectory(), "public");

			StaticContent.HostDirectory(path, parameters, ref response);
		}
	}
}
