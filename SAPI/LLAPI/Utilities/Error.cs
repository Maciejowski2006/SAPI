using System.Net;
using SAPI.API.Utilities;

// Disable warnings about using obsolete methods
#pragma warning disable CS0618

namespace SAPI.LLAPI.Utilities
{
	public static class Error
	{
		/// <summary>
		/// Wrapper to HighLevel implementation
		/// </summary>
		/// <param name="httpStatus">It's the status code send to client</param>
		/// <param name="response">Response ref you got from server - argument in Task()</param>
		public static void Page(HttpStatus httpStatus, ref HttpListenerRequest request, ref HttpListenerResponse response)
		{
			Packet packet = new (ref request, ref response, null);
			API.Utilities.Error.Page(httpStatus, ref packet);
		}
	}
}