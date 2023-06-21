using System.Net;
using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints
{
	public class Cookies : Endpoint
	{
		public override string url { get; } = "cookies";
		protected override void Get(ref Packet packet)
		{
			if (SAPI.API.Utilities.Cookies.CheckForCookie("visit", out Cookie cookie, ref packet))
			{
				SAPI.API.Utilities.Cookies.GiveCookie("visit", "false", ref packet);
				Error.ErrorPageResponse(HttpStatus.OK, ref packet);
			}
			else
			{
				SAPI.API.Utilities.Cookies.GiveCookie("visit", "true", ref packet);
				Error.ErrorPageResponse(HttpStatus.Forbidden, ref packet);
			}
		}
	}
}
