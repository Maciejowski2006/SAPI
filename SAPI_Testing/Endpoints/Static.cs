using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities.StaticContent;
namespace Testing.Endpoints
{
	public class Static : IEndpoint
	{
		public string url { get; } = "static";
		public Method method { get; } = Method.GET;
		public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
		{
			string file = Path.Combine(Directory.GetCurrentDirectory(), "linus.mov");
			
			StaticContent.FileResponse(file, ref response);
		}
	}
}
