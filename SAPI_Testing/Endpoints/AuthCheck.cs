using System.Net;
using SAPI;
using SAPI.API.Utilities;
using SAPI.Auth;

namespace Testing.Endpoints
{
	public class AuthCheck : Endpoint
	{
		public override string url { get; } = "auth-check";

		protected override void Post(HttpListenerContext context, Dictionary<string, string> parameters)
		{
			Json.Fetch(out AuthExt.NewAuth auth, context);
			Identity identity = new()
			{
				Identifier = auth.username,
				Password = auth.password
			};
			if (identity.Verify())
				Error.Page(HttpStatus.OK, context);
			else
				Error.Page(HttpStatus.Forbidden, context);
		}
	}
}