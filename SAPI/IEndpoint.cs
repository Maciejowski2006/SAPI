using System.Net;
using SAPI.LLAPI.Utilities;
using SAPI.Utilities;

namespace SAPI.Endpoints
{
	public interface IEndpoint
	{
		string url { get; }

		/// <summary>
		/// In task you execute code to satisfy the request
		/// </summary>
		/// <param name="request">Request info from server</param>
		/// <param name="response">Response is used to communicate to client</param>
		/// <param name="parameters">List of parameters provided from dynamic endpoint</param>
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
					Options(ref packet);
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
		protected virtual void Options(ref Packet packet)
		{
		}
		protected virtual void Head(ref Packet packet)
		{
		}
	}
}