using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace SAPI.Utilities
{
	public enum HttpStatus
	{
		OK,
		UseProxy,
		BadRequest,
		Unauthorized,
		Forbidden,
		NotFound,
		NotAcceptable,
		ProxyAuthenticationRequired,
		InternalServerError,
		NotImplemented,
		BadGateway,
		ServiceUnavailable
	}
	
	public class Utilities
	{
		static Dictionary<HttpStatus, string> httpStatusNames = new()
		{
			{HttpStatus.OK, "OK"},
			{HttpStatus.UseProxy, "Use Proxy"},
			{HttpStatus.BadRequest, "Bad Request"},
			{HttpStatus.Unauthorized, "Unauthorized"},
			{HttpStatus.Forbidden, "Forbidden"},
			{HttpStatus.NotFound, "Not Found"},
			{HttpStatus.NotAcceptable, "Not Acceptable"},
			{HttpStatus.ProxyAuthenticationRequired, "Proxy Authentication Required"},
			{HttpStatus.InternalServerError, "Internal Server Error"},
			{HttpStatus.NotImplemented, "Not Implemented"},
			{HttpStatus.BadGateway, "Bad Gateway"},
			{HttpStatus.ServiceUnavailable, "Service Unavailable"}
		};
		static Dictionary<HttpStatus, int> httpStatusCodes = new()
		{
			{HttpStatus.OK, 200},
			{HttpStatus.UseProxy, 305},
			{HttpStatus.BadRequest, 400},
			{HttpStatus.Unauthorized, 401},
			{HttpStatus.Forbidden, 403},
			{HttpStatus.NotFound, 404},
			{HttpStatus.NotAcceptable, 406},
			{HttpStatus.ProxyAuthenticationRequired, 407},
			{HttpStatus.InternalServerError, 500},
			{HttpStatus.NotImplemented, 501},
			{HttpStatus.BadGateway, 502},
			{HttpStatus.ServiceUnavailable, 503}
		};
		
		/// <summary>
		/// Responds to client with HTML page. Execute at the end of the task.
		/// </summary>
		/// <param name="page">Link to your .html file</param>
		/// <param name="response">Response ref you got from server - argument in Task()</param>
		public static void HtmlResponse(string page, ref HttpListenerResponse response)
		{
			byte[] data = Encoding.UTF8.GetBytes(page);
			response.ContentEncoding = Encoding.UTF8;
			response.ContentType = "text/html";
			response.ContentLength64 = data.LongLength;
			response.StatusCode = 200;
			
			response.OutputStream.Write(data, 0, data.Length);
		}
		/// <summary>
		/// Responds to client with JSON object. Execute at the end of the task.
		/// </summary>
		/// <param name="json">Add here your object to send with json</param>
		/// <param name="response">Response ref you got from server - argument in Task()</param>
		public static void JsonResponse<T>(T json, ref HttpListenerResponse response)
		{
			string serializedJson = JsonConvert.SerializeObject(json);
			byte[] data = Encoding.UTF8.GetBytes(serializedJson);
			response.ContentEncoding = Encoding.UTF8;
			response.ContentType = "application/josn";
			response.ContentLength64 = data.LongLength;
			response.StatusCode = 200;
			
			response.OutputStream.Write(data, 0, data.Length);
		}
		
		/// <summary>
		/// Shows an error page.
		/// </summary>
		/// <param name="httpStatus">It's the status code send to client</param>
		/// <param name="response">Response ref you got from server - argument in Task()</param>
		public static void Error(HttpStatus httpStatus, ref HttpListenerResponse response)
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

			byte[] data = Encoding.UTF8.GetBytes(page);
			response.ContentEncoding = Encoding.UTF8;
			response.ContentType = "text/html";
			response.ContentLength64 = data.LongLength;
			response.StatusCode = statusCode;
			
			response.OutputStream.Write(data, 0, data.Length);
		}
	}
}

