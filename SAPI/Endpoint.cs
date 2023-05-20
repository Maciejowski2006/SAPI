using System.Net;

namespace SAPI
{
	public abstract class Endpoint
	{
		public abstract string url { get; }

		public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
		{
			Packet packet = new(ref request, ref response, parameters);
			Method method = Enum.Parse<Method>(request.HttpMethod);
			switch (method)
			{
				case Method.GET:
				{
					Get(ref packet);
					break;
				}
				case Method.POST:
				{
					Post(ref packet);
					break;
				}
				case Method.PUT:
				{
					Put(ref packet);
					break;
				}
				case Method.PATCH:
				{
					Patch(ref packet);
					break;
				}
				case Method.DELETE:
				{
					Delete(ref packet);
					break;
				}
				case Method.OPTIONS:
				{
					Options(ref packet, new CorsOptions());
					break;
				}
				case Method.HEAD:
				{
					Head(ref packet);
					break;
				}
			}
		}

		protected virtual void Get(ref Packet packet)
		{
		}

		protected virtual void Post(ref Packet packet)
		{
		}

		protected virtual void Put(ref Packet packet)
		{
		}

		protected virtual void Patch(ref Packet packet)
		{
		}

		protected virtual void Delete(ref Packet packet)
		{
		}

		protected virtual void Options(ref Packet packet, CorsOptions corsOptions)
		{
		}

		protected virtual void Head(ref Packet packet)
		{
		}
	}
}