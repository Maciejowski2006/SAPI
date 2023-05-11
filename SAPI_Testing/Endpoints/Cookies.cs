using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints
{
	public class Cookies : IEndpoint
	{
		public string url { get; set; } = "cookies";
		public Method method { get; set; } = Method.GET;
		public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
		{
			if (SAPI.Utilities.Cookies.CheckForCookie("visit", out Cookie cookie, ref request))
			{
				SAPI.Utilities.Cookies.GiveCookie("visit", "false", ref response);
				Error.ErrorPageResponse(HttpStatus.OK, ref response);
			}
			else
			{
				SAPI.Utilities.Cookies.GiveCookie("visit", "true", ref response);
				Error.ErrorPageResponse(HttpStatus.Forbidden, ref response);
			}
		}
	}
}
