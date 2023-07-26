using SAPI;

namespace Testing.Endpoints
{
	public class Exception : Endpoint
	{
		public override string url { get; } = "exception";

		protected override void Get(ref Packet packet)
		{
			int.Parse("hello");
		}
	}
}