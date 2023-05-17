using System.Net;
using System.Text;
using SAPI.Internal;

namespace SAPI.Utilities
{
	public record BasicAuthCredentials(string username, string password);

	public static class Auth
	{
		/// TODO: Remove this method after rewrite
		/// <summary>
		/// Checks if user with provided API key exists.
		/// </summary>
		/// <param name="keys">List of all API keys authorized</param>
		/// <param name="headerName">Name of the authorization header(the OpenAPI 3.0 specification says the default should be "X-Api-Key"</param>
		/// <param name="request">Request ref you got from server - argument in Task()</param>
		public static bool CheckForKey(List<string> keys, string headerName, ref HttpListenerRequest request)
		{
			try
			{
				foreach (var key in keys)
					if (request.Headers.Get(headerName).Contains(key))
						return true;

				return false;
			}
			catch
			{
				Console.WriteLine($"Request does not have {headerName}.");
				return false;
			}
		}
		
		/// <summary>
		/// Checks if user with provided API key exists.
		/// </summary>
		/// <param name="keys">List of all API keys authorized</param>
		/// <param name="headerName">Name of the authorization header(the OpenAPI 3.0 specification says the default should be "X-Api-Key"</param>
		/// <param name="request">Request ref you got from server - argument in Task()</param>
		public static bool CheckForKey(List<string> keys, string headerName, ref Packet packet)
		{
			try
			{
				foreach (var key in keys)
					if (packet.Request.Headers.Get(headerName).Contains(key))
						return true;

				return false;
			}
			catch
			{
				Console.WriteLine($"Request does not have {headerName}.");
				return false;
			}
		}

		/// TODO: Remove this method after rewrite
		/// <summary>
		/// Checks if user with provided credentials exists.
		/// </summary>
		/// <param name="credentialsList">List of all usernames and passwords authorized</param>
		/// <param name="request">Request ref you got from server - argument in Task()</param>
		public static bool CheckForUserPass(List<BasicAuthCredentials> credentialsList, ref HttpListenerRequest request)
		{
			try
			{
				if (GetBasicAuthCredentials(out BasicAuthCredentials credentials, ref request))
				{
					foreach (BasicAuthCredentials _credentials in credentialsList)
						if (string.Equals(_credentials.username, credentials.username) && string.Equals(_credentials.password, credentials.password))
							return true;
				}
			}
			catch
			{
				Internals.WriteLine($"Request does not have Authorization header.");
			}

			return false;
		}
		/// TODO: Doesn't work - investigate
		/// <summary>
		/// Checks if user with provided credentials exists.
		/// </summary>
		/// <param name="credentialsList">List of all usernames and passwords authorized</param>
		/// <param name="request">Request ref you got from server - argument in Task()</param>
		public static bool CheckForUserPass(List<BasicAuthCredentials> credentialsList, ref Packet packet)
		{
			try
			{
				if (GetBasicAuthCredentials(out BasicAuthCredentials credentials, ref packet))
				{
					foreach (BasicAuthCredentials _credentials in credentialsList)
						if (string.Equals(_credentials.username, credentials.username) && string.Equals(_credentials.password, credentials.password))
							return true;
				}
			}
			catch
			{
				Internals.WriteLine($"Request does not have Authorization header.");
			}

			return false;
		}

		/// TODO: Remove this method after rewrite
		/// <summary>
		/// Returns Basic auth credentials.
		/// </summary>
		/// <param name="credentials">Variable contains passed user credentials</param>
		/// <param name="request">Pass from Task()</param>
		public static bool GetBasicAuthCredentials(out BasicAuthCredentials? credentials, ref HttpListenerRequest request)
		{
			credentials = null;
			try
			{
				if (request.Headers.Get("Authorization").Contains("Basic "))
				{
					string authData = request.Headers.GetValues("Authorization").GetValue(0).ToString().Substring(6);
					
					byte[] decodedBase64 = Convert.FromBase64String(authData);
					
					string[] auth = Encoding.UTF8.GetString(decodedBase64).Split(':');
					credentials = new (auth[0], auth[1]);
				}
				
				return true;
			}
			catch
			{
				return false;
			}
		}
		
		/// <summary>
		/// Returns Basic auth credentials.
		/// </summary>
		/// <param name="credentials">Variable contains passed user credentials</param>
		/// <param name="request">Pass from Task()</param>
		public static bool GetBasicAuthCredentials(out BasicAuthCredentials? credentials, ref Packet packet)
		{
			credentials = null;
			try
			{
				if (packet.Request.Headers.Get("Authorization").Contains("Basic "))
				{
					string authData = packet.Request.Headers.GetValues("Authorization").GetValue(0).ToString().Substring(6);
					
					byte[] decodedBase64 = Convert.FromBase64String(authData);
					
					string[] auth = Encoding.UTF8.GetString(decodedBase64).Split(':');
					credentials = new (auth[0], auth[1]);
				}
				
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}