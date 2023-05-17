using System.Net;
using System.Text;

namespace SAPI.LLAPI.Utilities
{
	public static class Html
	{
		/// TODO: Remove this method after rewrite
		/// <summary>
		/// Responds to client with HTML page.
		/// </summary>
		/// <param name="page">Link to your .html file</param>
		/// <param name="response">Response ref you got from server - argument in Task()</param>
		[Obsolete("HtmlResponse is deprecated, if you want to host SSR websites, please use Wolf (https://github.com/Maciejowski2006/Wolf)")]
		public static void HtmlResponse(string page, ref HttpListenerResponse response, int statusCode = 200)
		{
			var data = Encoding.UTF8.GetBytes(page);
			response.ContentEncoding = Encoding.UTF8;
			response.ContentType = "text/html";
			response.ContentLength64 = data.LongLength;
			response.StatusCode = statusCode;

			response.OutputStream.Write(data, 0, data.Length);
		}
		/// <summary>
		/// Responds to client with HTML page.
		/// </summary>
		/// <param name="page">Link to your .html file</param>
		/// <param name="response">Response ref you got from server - argument in Task()</param>
		[Obsolete("HtmlResponse is deprecated, if you want to host SSR websites, please use Wolf (https://github.com/Maciejowski2006/Wolf)")]
		public static void HtmlResponse(string page, ref Packet packet, int statusCode = 200)
		{
			byte[] data = Encoding.UTF8.GetBytes(page);
			packet.Response.ContentEncoding = Encoding.UTF8;
			packet.Response.ContentType = "text/html";
			packet.Response.ContentLength64 = data.LongLength;
			packet.Response.StatusCode = statusCode;

			packet.Response.OutputStream.Write(data, 0, data.Length);
		}
	}
}