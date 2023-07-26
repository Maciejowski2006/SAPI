using System.Net;
using SAPI.API.Utilities;
using SAPI.LLAPI;
using Debug = SAPI.API.Debug;

namespace SAPI
{
	public abstract class Endpoint
	{
		public abstract string url { get; }

		public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
		{
			Packet packet = new(ref request, ref response, parameters);
			Method method = Enum.Parse<Method>(request.HttpMethod);

			if (method != Method.OPTIONS)
			{
				string methodCapitalized = $"{method.ToString()[0].ToString().ToUpper()}{method.ToString().Substring(1).ToLower()}";

				if (!EndpointManager.CheckForDefinedMethod(methodCapitalized, GetType()))
				{
					Error.Page(HttpStatus.MethodNotAllowed, ref packet);
					return;
				}
			}

			try
			{
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
			catch (Exception e)
			{
				Debug.Error($"Error: {e.Message}");
				Debug.Error($"Stack Trace: {e.StackTrace}");
				Error.Page(HttpStatus.InternalServerError, ref packet);
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

		protected virtual void Options(ref Packet packet, CorsOptions cors)
		{
			packet.Response.AddHeader("Access-Control-Allow-Origin", cors.AllowOrigin);
			if (cors.AllowCredentials)
				packet.Response.AddHeader("Access-Control-Allow-Credentials", "true");
			
			packet.Response.AddHeader("Access-Control-Max-Age", cors.MaxAge.ToString());
			if (cors.AllowHeaders.Length > 0)
			{
				string allowHeaders = String.Join(", ", cors.AllowHeaders);
				packet.Response.AddHeader("Access-Control-Allow-Headers", allowHeaders);
			}

			packet.Response.AddHeader("Access-Control-Allow-Methods", EndpointManager.GetDefinedMethods(GetType()));
		}

		protected void Head(ref Packet packet)
		{
			Get(ref packet);
		}
	}
}