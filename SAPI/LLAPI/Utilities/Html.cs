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
		public static void HtmlResponse(string page, ref Packet packet, int statusCode = 200)
		{
			byte[] data = Encoding.UTF8.GetBytes(page);
			packet.Response.ContentEncoding = Encoding.UTF8;
			packet.Response.ContentType = "text/html";
			packet.Response.ContentLength64 = data.LongLength;
			packet.Response.StatusCode = statusCode;

			if (packet.Request.HttpMethod != "HEAD")
			{
				packet.Response.OutputStream.Write(data, 0, data.Length);
			}
		}
	}
}