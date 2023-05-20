using System.Net;

namespace SAPI
{
	[Obsolete("Use Endpoint class instead")]
	public interface IEndpoint
	{
		string url { get; }

		public sealed void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
		{
			Packet packet = new(ref request, ref response, parameters);
			Method method = Enum.Parse<Method>(request.HttpMethod);
			switch (method)
			{
				case Method.GET:
				{
					Get(ref packet, Get);
					break;
				}
				case Method.POST:
				{
					Post(ref packet, Post);
					break;
				}
				case Method.PUT:
				{
					Put(ref packet, Put);
					break;
				}
				case Method.PATCH:
				{
					Patch(ref packet, Patch);
					break;
				}
				case Method.DELETE:
				{
					Delete(ref packet, Delete);
					break;
				}
				case Method.OPTIONS:
				{
					Options(ref packet, new CorsOptions(), Options);
					break;
				}
				case Method.HEAD:
				{
					Head(ref packet, Head);
					break;
				}
			}
		}

		public delegate void BaseGet(ref Packet packet, BaseGet baseMethod = null);
		protected void Get(ref Packet packet, BaseGet baseMethod)
		{
		}

		public delegate void BasePost(ref Packet packet, BasePost baseMethod = null);
		protected void Post(ref Packet packet, BasePost baseMethod)
		{
		}

		public delegate void BasePut(ref Packet packet, BasePut baseMethod = null);
		protected void Put(ref Packet packet, BasePut baseMethod)
		{
		}

		public delegate void BasePatch(ref Packet packet, BasePatch baseMethod = null);
		protected void Patch(ref Packet packet, BasePatch baseMethod)
		{
		}

		public delegate void BaseDelete(ref Packet packet, BaseDelete methodBase);
		protected void Delete(ref Packet packet, BaseDelete baseMethod)
		{
		}

		public delegate void BaseOptions(ref Packet packet, CorsOptions corsOptions, BaseOptions baseMethod = null);
		protected void Options(ref Packet packet, CorsOptions corsOptions, BaseOptions baseMethod)
		{
		}

		public delegate void BaseHead(ref Packet packet, BaseHead baseMethod = null);
		protected void Head(ref Packet packet, BaseHead baseMethod)
		{
		}
	}
}