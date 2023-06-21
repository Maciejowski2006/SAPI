﻿using System.Net;

namespace SAPI.API.Utilities
{
	public static class Cookies
	{
		/// <summary>
		/// Check if cookie with specified name exists
		/// </summary>
		/// <param name="cookieName">Name of the cookie to check</param>
		/// <param name="cookie">Return Cookie</param>
		/// <param name="request">Pass from Task()</param>
		/// <returns>Does cookie exists?</returns>
		public static bool CheckForCookie(string cookieName, out Cookie? cookie, ref Packet packet)
		{
			cookie = packet.Request.Cookies[cookieName];

			return cookie != null;
		}

		/// <summary>
		/// Give cookie to client
		/// </summary>
		/// <param name="cookieName">Name of cookie to be saved</param>
		/// <param name="cookieValue">Value of cookie to be saved</param>
		/// <param name="response">Pass from Task()</param>
		public static void GiveCookie(string cookieName, string cookieValue, ref Packet packet)
		{
			Cookie cookie = new(cookieName, cookieValue);
			packet.Response.AppendCookie(cookie);
		}
	}
}