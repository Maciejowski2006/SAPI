using System.Net;

namespace SAPI
{
	public ref struct Packet
	{
		public HttpListenerRequest Request { get; init; }
		public HttpListenerResponse Response { get; init; }
		public readonly Dictionary<string, string> Parameters { get; init; }

		public Packet(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
		{
			Request = request;
			Response = response;
			Parameters = parameters;
		}
	}
}