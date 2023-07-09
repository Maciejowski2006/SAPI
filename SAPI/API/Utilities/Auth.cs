using System.Text;

namespace SAPI.API.Utilities
{
	public record BasicCredentials(string Username, string? Password, string? HashedPassword = null)
	{
		public string Username = Username;
		public string? Password = Password;
		public string? HashedPassword = HashedPassword;
	}

	public static class Auth
	{
		/// <summary>
		/// Checks if user with provided API key exists.
		/// </summary>
		/// <param name="keys">List of all API keys authorized</param>
		/// <param name="headerName">Name of the authorization header(OpenAPI 3.0 specification suggests the default should be "x-api-key"</param>
		/// <param name="packet">Packet ref you got from server</param>
		public static bool CheckForApiKey(List<string> keys, string headerName, ref Packet packet)
		{
			try
			{
				if (GetApiKey(out string? _key, headerName, ref packet))
					foreach (var key in keys)
					{
						if (_key == key)
							return true;
					}

				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Checks if user with provided credentials exists.
		/// </summary>
		/// <param name="credentialsList">List of all usernames and passwords authorized</param>
		/// <param name="hashingFunction">Password hashing algorithm. Takes un-hashed password as parameter, returns hashed password</param>
		/// <param name="packet">Packet ref you got from server</param>
		public static bool CheckForBasicCredentials(List<BasicCredentials> credentialsList, Func<string, string> hashingFunction, ref Packet packet)
		{
			try
			{
				if (GetBasicCredentials(out BasicCredentials? credentials, ref packet))
				{
					credentials.HashedPassword = hashingFunction(credentials.Password);
					foreach (BasicCredentials _credentials in credentialsList)
					{
						if (string.Equals(_credentials.Username, credentials.Username) && string.Equals(_credentials.HashedPassword, credentials.HashedPassword))
							return true;
					}
				}
			}
			catch
			{
				Debug.Log("Request does not have Authorization header.");
			}

			return false;
		}
		
		public static bool GetApiKey(out string? key, string headerName, ref Packet packet)
		{
			key = packet.Request.Headers.Get(headerName);

			if (key is null)
				return false;

			return true;
		}

		/// <summary>
		/// Returns Basic auth credentials.
		/// </summary>
		/// <param name="credentials">Variable contains passed user credentials</param>
		/// <param name="request">Packet ref you got from server</param>
		public static bool GetBasicCredentials(out BasicCredentials? credentials, ref Packet packet)
		{
			credentials = null;
			try
			{
				if (packet.Request.Headers.Get("Authorization").Contains("Basic "))
				{
					string authData = packet.Request.Headers.GetValues("Authorization").GetValue(0).ToString().Substring(6);

					byte[] decodedBase64 = Convert.FromBase64String(authData);

					string[] auth = Encoding.UTF8.GetString(decodedBase64).Split(':');
					credentials = new(auth[0], auth[1]);
					return true;
				}

				return false;
			}
			catch
			{
				Debug.Log("Request does not have Authorization header.");
			}
			return false;
		}
	}
}