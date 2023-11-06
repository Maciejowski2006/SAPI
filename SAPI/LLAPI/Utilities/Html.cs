using System.Net;
using System.Text;

namespace SAPI.LLAPI.Utilities
{
	public static class Html
	{
		/// <summary>
		/// Responds to client with HTML page.
		/// </summary>
		/// <param name="page">Link to your .html file</param>
		/// <param name="packet">Packet ref you got from server</param>
		/// <param name="statusCode">Status code to send</param>
		[Obsolete("HtmlResponse is deprecated, if you want to host SSR websites, please use Wolf (https://github.com/Maciejowski2006/Wolf)")]
		public static void HtmlResponse(string page, HttpListenerContext context, int statusCode = 200)
		{
			byte[] data = Encoding.UTF8.GetBytes(page);
			context.Response.ContentEncoding = Encoding.UTF8;
			context.Response.ContentType = "text/html";
			context.Response.ContentLength64 = data.LongLength;
			context.Response.StatusCode = statusCode;

			if (context.Request.HttpMethod != "HEAD")
			{
				context.Response.OutputStream.Write(data, 0, data.Length);
			}
		}
	}
}