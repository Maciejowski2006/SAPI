using System.Net;
using System.Text;

namespace SAPI.Utilities.Auth
{
	public record BasicAuthCredentials(string username, string password);
	
	public class Auth
	{
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
			foreach (string key in keys)
			{
				if (request.Headers.Get(headerName).Contains(key))
					return true;
			}
			return false;
		}
		catch
		{
			Console.WriteLine($"Request does not have {headerName}.");
			return false;
		}
	}
	/// <summary>
	/// Checks if user with provided credentials exists.
	/// </summary>
	/// <param name="credentialsList">List of all usernames and passwords authorized</param>
	/// <param name="request">Request ref you got from server - argument in Task()</param>
	public static bool CheckForUserPass(List<BasicAuthCredentials> credentialsList, ref HttpListenerRequest request)
	{
		try
		{
			if (request.Headers.Get("Authorization").Contains("Basic "))
			{
				// Get data from header end remove "Basic " at the beginning
				string authData = request.Headers.GetValues("Authorization").GetValue(0).ToString().Substring(6);

				// Convert from Base64
				byte[] decodedBase64 = Convert.FromBase64String(authData);
			
				// Encode in UTF-8
				string[] auth = Encoding.UTF8.GetString(decodedBase64).Split(':');
				BasicAuthCredentials userCredentials = new BasicAuthCredentials(auth[0], auth[1]);

				foreach (BasicAuthCredentials credentials in credentialsList)
				{
					if (String.Equals(userCredentials.username, credentials.username) && String.Equals(userCredentials.password, credentials.password))
						return true;
				}
			}
		}
		catch
		{
			Console.WriteLine($"Request does not have Authorization header.");
			return false;
		}
		return false;
	}
	}
}
