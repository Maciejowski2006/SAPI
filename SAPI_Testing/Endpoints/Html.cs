using SAPI;

namespace Testing.Endpoints
{
	public class Html : IEndpoint
	{
		public string url { get; } = "html";

		public void Get(ref Packet packet)
		{
			SAPI.LLAPI.Utilities.Html.HtmlResponse("test", ref packet);
		}

		public void Options(ref Packet packet, CorsOptions corsOptions, IEndpoint.BaseOptions baseMethod)
		{
			corsOptions = new CorsBuilder().AllowOrigin(AccessControlAllowOrigin.All).MaxAge(86400).Build();

			baseMethod(ref packet, corsOptions);
		}
	}
}