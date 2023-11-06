using System.Net;
using SAPI;
using SAPI.API.Utilities;
using SAPI.Auth;

namespace Testing.Endpoints
{
	#pragma warning disable CS0618
	public class AuthExt : Endpoint
	{
		public override string url { get; } = "auth-ext";
        
		protected override void Get(ref Packet packet)
		{
			SAPI.LLAPI.Utilities.Html.HtmlResponse("<h1>GOOD</h1>", ref packet);
		}

		protected override void Post(ref Packet packet)
		{
			Json.Fetch(out NewAuth auth, ref packet);
			
			Identity identity = new()
			{
				Identifier = auth.username,
				Password = auth.password
			};
			if (identity.Create())
				Error.Page(HttpStatus.OK, ref packet);
			else
				Error.Page(HttpStatus.BadRequest, ref packet);
			
			Session.GenerateSessionToken(identity, packet);
			
			Cookie cookie = new("test", "test")
			{
				Expires = DateTime.UtcNow.AddDays(1)
			};
			packet.Response.AppendCookie(cookie);
		}

		public record NewAuth(string username, string password);
	}
	
}