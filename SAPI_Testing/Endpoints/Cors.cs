using System.Net;
using SAPI;

namespace Testing.Endpoints
{
	public class Cors : Endpoint
	{
		public override string url { get; } = "cors";

		protected override void Options(HttpListenerContext context, Dictionary<string, string> parameters, CorsOptions cors)
		{
			cors = new CorsBuilder().AllowOrigin(AccessControlAllowOrigin.SameOrigin).AllowCredentials().MaxAge(86000).Build();

			base.Options(context, parameters, cors);
		}
	}
}