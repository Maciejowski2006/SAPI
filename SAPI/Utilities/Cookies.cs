using System.Net;

namespace SAPI.Utilities.Cookies
{
	public static class Cookies
	{
		public static bool CheckForCookie(string cookieName, out Cookie? cookie, ref HttpListenerRequest request)
		{
			cookie = request.Cookies[cookieName];

			return cookie != null;
		}
		public static void GiveCookie(string cookieName, string cookieValue, ref HttpListenerResponse response)
		{
			Cookie cookie = new(cookieName, cookieValue);
			response.AppendCookie(cookie);
		}
	}
}
