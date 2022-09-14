using System.Net;
using SAPI.Endpoints;

namespace Testing.Endpoints
{
	public class GetFromAPI : Endpoint
	{

		public GetFromAPI(string url, Method method) : base(url, method) { }
		public override void Task(ref HttpListenerRequest request, ref HttpListenerResponse response)
		{
			Console.WriteLine("Get request from api recorded");
		}
	}

}