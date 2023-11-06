using SAPI;
using SAPI.API.Utilities;
using SAPI.Auth;

namespace Testing.Endpoints
{
	public class AuthCheck : Endpoint
	{
		public override string url { get; } = "auth-check";

		protected override void Post(ref Packet packet)
		{
			Json.Fetch(out AuthExt.NewAuth auth, ref packet);
			Identity identity = new()
			{
				Identifier = auth.username,
				Password = auth.password
			};
			if (identity.Verify())
				Error.Page(HttpStatus.OK, ref packet);
			else
				Error.Page(HttpStatus.Forbidden, ref packet);
		}
	}
}