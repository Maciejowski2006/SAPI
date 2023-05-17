using System.Net;

namespace SAPI
{
	public ref struct Packet
	{
		public HttpListenerRequest Request { get; init; }
		public HttpListenerResponse Response { get; init; }
		public readonly Dictionary<string, string> Paramters { get; init; }

		public Packet(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> paramters)
		{
			Request = request;
			Response = response;
			Paramters = paramters;
		}
	}
}