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
        
		protected override void Get(HttpListenerContext context, Dictionary<string, string> parameters)
		{
			SAPI.LLAPI.Utilities.Html.HtmlResponse("<h1>GOOD</h1>", context);
		}

		protected override void Post(HttpListenerContext context, Dictionary<string, string> parameters)
		{
			Json.Fetch(out NewAuth auth, context);
			
			Identity identity = new()
			{
				Identifier = auth.username,
				Password = auth.password
			};
			if (identity.Create())
				Error.Page(HttpStatus.OK, context);
			else
				Error.Page(HttpStatus.BadRequest, context);
			
			Session.GenerateSessionToken(identity, context);
			
			Cookie cookie = new("test", "test")
			{
				Expires = DateTime.UtcNow.AddDays(1)
			};
			context.Response.AppendCookie(cookie);
		}

		public record NewAuth(string username, string password);
	}
	
}