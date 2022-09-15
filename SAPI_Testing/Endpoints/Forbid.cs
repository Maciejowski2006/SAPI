using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;
namespace Testing.Endpoints
{
	public class Forbid : Endpoint
	{
		public Forbid(string url, Method method) : base(url, method) { }
		public override void Task(ref HttpListenerRequest request, ref HttpListenerResponse response)
		{
			Utilities.Error(HttpStatus.NotFound, ref response);
		}
	}
}


