using System.Net;
using SAPI.API.Utilities;
using SAPI.LLAPI;
using Debug = SAPI.API.Debug;

namespace SAPI
{
	public abstract class Endpoint
	{
		public abstract string url { get; }

		public void Task(HttpListenerContext context, Dictionary<string, string> parameters)
		{
			Method method = Enum.Parse<Method>(context.Request.HttpMethod);

			if (method != Method.OPTIONS)
			{
				string methodCapitalized = $"{method.ToString()[0].ToString().ToUpper()}{method.ToString().Substring(1).ToLower()}";

				if (!EndpointManager.CheckForDefinedMethod(methodCapitalized, GetType()))
				{
					Error.Page(HttpStatus.MethodNotAllowed, context);
					return;
				}
			}

			try
			{
				switch (method)
				{
					case Method.GET:
					{
						Get(context, parameters);
						break;
					}
					case Method.POST:
					{
						Post(context, parameters);
						break;
					}
					case Method.PUT:
					{
						Put(context, parameters);
						break;
					}
					case Method.PATCH:
					{
						Patch(context, parameters);
						break;
					}
					case Method.DELETE:
					{
						Delete(context, parameters);
						break;
					}
					case Method.OPTIONS:
					{
						Options(context, parameters, new CorsOptions());
						break;
					}
					case Method.HEAD:
					{
						Head(context, parameters);
						break;
					}
				}
			}
			catch (Exception e)
			{
				Debug.Error($"Error: {e.Message}");
				Debug.Error($"Stack Trace: {e.StackTrace}");
				Error.Page(HttpStatus.InternalServerError, context);
			}
		}

		protected virtual void Get(HttpListenerContext context, Dictionary<string, string> parameters)
		{
		}

		protected virtual void Post(HttpListenerContext context, Dictionary<string, string> parameters)
		{
		}

		protected virtual void Put(HttpListenerContext context, Dictionary<string, string> parameters)
		{
		}

		protected virtual void Patch(HttpListenerContext context, Dictionary<string, string> parameters)
		{
		}

		protected virtual void Delete(HttpListenerContext context, Dictionary<string, string> parameters)
		{
		}

		protected virtual void Options(HttpListenerContext context, Dictionary<string, string> parameters, CorsOptions cors)
		{
			context.Response.AddHeader("Access-Control-Allow-Origin", cors.AllowOrigin);
			if (cors.AllowCredentials)
				context.Response.AddHeader("Access-Control-Allow-Credentials", "true");
			
			context.Response.AddHeader("Access-Control-Max-Age", cors.MaxAge.ToString());
			if (cors.AllowHeaders.Length > 0)
			{
				string allowHeaders = String.Join(", ", cors.AllowHeaders);
				context.Response.AddHeader("Access-Control-Allow-Headers", allowHeaders);
			}

			context.Response.AddHeader("Access-Control-Allow-Methods", EndpointManager.GetDefinedMethods(GetType()));
		}

		private void Head(HttpListenerContext context, Dictionary<string, string> parameters)
		{
			Get(context, parameters);
		}
	}
}