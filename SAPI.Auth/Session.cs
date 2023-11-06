using System.Net;
using SAPI.API.Utilities;

namespace SAPI.Auth
{
	public static class Session
	{
		public static List<SessionToken> SessionTokens = new();

		public static void GenerateSessionToken(Identity identity, Packet packet)
		{
			SessionToken token = new()
			{
				Token = Guid.NewGuid(),
				IdentityId = identity.Id,
				ValidationTime = DateTime.UtcNow
			};

			Console.WriteLine(token.Token.ToString());

			
			
			
			//packet.Response.SetCookie(new Cookie("SESSION", token.Token.ToString()));
			// Cookies.GiveCookie(new Cookie("SESSION", token.Token.ToString()), ref packet);
		}
	}

	public struct SessionToken
	{
		public Guid Token { get; init; }
		public int IdentityId { get; init; }
		public DateTime ValidationTime { get; init; }
	}
}