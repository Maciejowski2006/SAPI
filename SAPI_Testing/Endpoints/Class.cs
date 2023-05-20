using SAPI;
using SAPI.Utilities;

namespace Testing.Endpoints
{
	public class Class : Endpoint
	{
		public override string url { get; } = "class";

		protected override void Get(ref Packet packet)
		{
			Error.ErrorPageResponse(HttpStatus.UseProxy, ref packet);
		}
	}
}