using SAPI;

namespace Testing.Endpoints
{
	public class Cors : Endpoint
	{
		public override string url { get; } = "cors";

		protected override void Options(ref Packet packet, CorsOptions corsOptions)
		{
			corsOptions = new()
			{
				AllowOrigin = AccessControlAllowOrigin.ThisOrigin.ToString(),
			};

			base.Options(ref packet, corsOptions);
		}
	}
}