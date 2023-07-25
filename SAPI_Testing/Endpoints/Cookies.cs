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
			if (packet.Parameters["passcode"] == "letmein")
			{
				SAPI.API.Utilities.Cookies.GiveCookie(new Cookie("visit", "true"), ref packet);
				Error.Page(HttpStatus.OK, ref packet);
			}
			else
			{
				Error.Page(HttpStatus.Unauthorized, ref packet);
			}
				
				
			if (SAPI.API.Utilities.Cookies.CheckForCookie("visit", out Cookie cookie, ref packet))
			{
				SAPI.API.Utilities.Cookies.GiveCookie(new ("visit", "false"), ref packet);
				Error.Page(HttpStatus.OK, ref packet);
			}
			else
			{
				SAPI.API.Utilities.Cookies.GiveCookie(new ("visit", "true"), ref packet);
				Error.Page(HttpStatus.Forbidden, ref packet);
			}
		}
	}
}
