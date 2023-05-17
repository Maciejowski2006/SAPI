using SAPI;
using SAPI.Endpoints;

namespace Testing.Endpoints
{
	public class Html : IEndpoint
	{
		public string url { get; } = "html";
		public Method method { get; } = Method.GET;

		public void Get(ref Packet packet)
		{
			SAPI.LLAPI.Utilities.Html.HtmlResponse("test", ref packet);
		}
	}
}