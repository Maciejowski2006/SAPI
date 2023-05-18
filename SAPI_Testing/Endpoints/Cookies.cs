using System.Net;
using SAPI;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints
{
	public class Cookies : IEndpoint
	{
		public string url { get; set; } = "cookies";
		public void Get(ref Packet packet)
		{
			if (SAPI.Utilities.Cookies.CheckForCookie("visit", out Cookie cookie, ref packet))
			{
				SAPI.Utilities.Cookies.GiveCookie("visit", "false", ref packet);
				Error.ErrorPageResponse(HttpStatus.OK, ref packet);
			}
			else
			{
				SAPI.Utilities.Cookies.GiveCookie("visit", "true", ref packet);
				Error.ErrorPageResponse(HttpStatus.Forbidden, ref packet);
			}
		}
	}
}
