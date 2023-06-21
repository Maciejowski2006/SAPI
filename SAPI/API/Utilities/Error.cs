using SAPI.LLAPI.Utilities;

// Disable warnings about using obsolete methods
#pragma warning disable CS0618

namespace SAPI.API.Utilities
{
	public static class Error
	{
		private static Dictionary<HttpStatus, string> httpStatusNames = new()
		{
			{ HttpStatus.OK, "OK" },
			{ HttpStatus.UseProxy, "Use Proxy" },
			{ HttpStatus.BadRequest, "Bad Request" },
			{ HttpStatus.Unauthorized, "Unauthorized" },
			{ HttpStatus.Forbidden, "Forbidden" },
			{ HttpStatus.NotFound, "Not Found" },
			{ HttpStatus.NotAcceptable, "Not Acceptable" },
			{ HttpStatus.ProxyAuthenticationRequired, "Proxy Authentication Required" },
			{ HttpStatus.InternalServerError, "Internal Server Error" },
			{ HttpStatus.NotImplemented, "Not Implemented" },
			{ HttpStatus.BadGateway, "Bad Gateway" },
			{ HttpStatus.ServiceUnavailable, "Service Unavailable" }
		};

		private static Dictionary<HttpStatus, int> httpStatusCodes = new()
		{
			{ HttpStatus.OK, 200 },
			{ HttpStatus.UseProxy, 305 },
			{ HttpStatus.BadRequest, 400 },
			{ HttpStatus.Unauthorized, 401 },
			{ HttpStatus.Forbidden, 403 },
			{ HttpStatus.NotFound, 404 },
			{ HttpStatus.NotAcceptable, 406 },
			{ HttpStatus.ProxyAuthenticationRequired, 407 },
			{ HttpStatus.InternalServerError, 500 },
			{ HttpStatus.NotImplemented, 501 },
			{ HttpStatus.BadGateway, 502 },
			{ HttpStatus.ServiceUnavailable, 503 }
		};
		
		/// <summary>
		/// Shows an error page.
		/// </summary>
		/// <param name="httpStatus">It's the status code send to client</param>
		/// <param name="response">Response ref you got from server - argument in Task()</param>
		public static void ErrorPageResponse(HttpStatus httpStatus, ref Packet packet)
		{
			string statusName = httpStatusNames[httpStatus];
			int statusCode = httpStatusCodes[httpStatus];

			string page =
				"<!DOCTYPE>" +
				"<html>" +
				"	<head>" +
				$"		<title>{statusCode} Error!</title>" +
				"		<style>" +
				"			h1, h3 {" +
				"				text-align: center;" +
				"				margin-block: 1.5rem;" +
				"				font-weight: 600;" +
				"			}" +
				"		</style>" +
				"	</head>" +
				"	<body>" +
				$"		<h1>{statusCode} {statusName}</h1>" +
				"		<hr>" +
				"		<h3>SAPI</h3>" +
				"	</body>" +
				"</html>";

			Html.HtmlResponse(page, ref packet, statusCode);
		}
	}
}