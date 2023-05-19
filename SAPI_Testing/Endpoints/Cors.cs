using SAPI;

namespace Testing.Endpoints
{
	public class Cors : IEndpoint
	{
		public string url { get; } = "cors";

		protected void Options(ref Packet packet, CorsOptions corsOptions, IEndpoint.BaseOptions baseMethod)
		{
			corsOptions = new()
			{
				AllowOrigin = AccessControlAllowOrigin.ThisOrigin.ToString(),
			};

			baseMethod(ref packet, corsOptions);
		}
	}
}