using System.Net;
using SAPI;

namespace Testing.Endpoints
{
	public class Html : Endpoint
	{
		public override string url { get; } = "html";

		protected override void Get(HttpListenerContext context, Dictionary<string, string> parameters)
		{
			SAPI.LLAPI.Utilities.Html.HtmlResponse("test", context);
		}

		protected override void Options(HttpListenerContext context, Dictionary<string, string> parameters, CorsOptions cors)
		{
			cors = new CorsBuilder().AllowOrigin(AccessControlAllowOrigin.All).MaxAge(86400).Build();

			base.Options(context, parameters, cors);
		}
	}
}