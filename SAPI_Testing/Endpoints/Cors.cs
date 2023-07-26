using SAPI;

namespace Testing.Endpoints
{
	public class Cors : Endpoint
	{
		public override string url { get; } = "cors";

		protected override void Options(ref Packet packet, CorsOptions cors)
		{
			cors = new CorsBuilder().AllowOrigin(AccessControlAllowOrigin.SameOrigin).AllowCredentials().MaxAge(86000).Build();

			base.Options(ref packet, cors);
		}
	}
}