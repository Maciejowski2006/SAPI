using SAPI;

namespace Testing.Endpoints
{
	public class Html : Endpoint
	{
		public override string url { get; } = "html";

		protected override void Get(ref Packet packet)
		{
			SAPI.LLAPI.Utilities.Html.HtmlResponse("test", ref packet);
		}

		protected override void Options(ref Packet packet, CorsOptions cors)
		{
			cors = new CorsBuilder().AllowOrigin(AccessControlAllowOrigin.All).MaxAge(86400).Build();

			base.Options(ref packet, cors);
		}
	}
}