using System.Net;
using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints
{
	public class Cookies : Endpoint
	{
		public override string url { get; } = "cookies";

		protected override void Get(HttpListenerContext context, Dictionary<string, string> parameters)
		{
			SAPI.API.Utilities.Cookies.GiveCookie(new Cookie("visit", "true"), context);
			Error.Page(HttpStatus.OK, context);
			/*if (parameters["passcode"] == "letmein")
			{
				SAPI.API.Utilities.Cookies.GiveCookie(new Cookie("visit", "true"), context);
				Error.Page(HttpStatus.OK, context);
			}
			else
			{
				Error.Page(HttpStatus.Unauthorized, context);
			}*/


			if (SAPI.API.Utilities.Cookies.CheckForCookie("visit", out Cookie cookie, context))
			{
				SAPI.API.Utilities.Cookies.GiveCookie(new("visit", "false"), context);
				Error.Page(HttpStatus.OK, context);
			}
			else
			{
				SAPI.API.Utilities.Cookies.GiveCookie(new("visit", "true"), context);
				Error.Page(HttpStatus.Forbidden, context);
			}
		}
	}
}