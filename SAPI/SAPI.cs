using System.Net;
using System.Text.RegularExpressions;
using SAPI.Internal;
using SAPI.LLAPI;
using SAPI.Utilities;
using Sentry;

namespace SAPI
{
	public class Server
	{
		private static HttpListener listener;
		private string url;
		private static bool requestResolved;
		private static List<Endpoint> endpoints;
		private static Regex dynamicRegex = new(":(.+?)(?:(?=/)|$)", RegexOptions.Compiled);

		/// <summary>
		/// Initalizes SAPI on default address: http://localhost:8000/
		/// </summary>
		/// <param name="url">Sets custom url - remember to put "/" at the end. If no parameter is provided, SAPI starts on default address</param>
		public Server()
		{
			Logger logAccess = new("access", Logger.LogType.Access);
			Logger logSystem = new("system", Logger.LogType.System);
			Internals.access = logAccess;
			Internals.system = logSystem;
			Config.system = logSystem;

			Config.Init();
			url = Config.ReadConfig().Url;

			
			listener = new HttpListener();
			listener.Prefixes.Add(url);
			endpoints = new List<Endpoint>();
			EndpointManager.FindAndMount(ref endpoints);
		}

		/// <summary>
		/// Starts the SAPI server. Execute at the end.
		/// </summary>
		public void Start()
		{
			if (Config.ReadConfig().EnableErrorReporting)
				using (SentrySdk.Init(o =>
				       {
					       o.Dsn = "https://024ccd09f43a47e889fbb78388d7b916@o1346432.ingest.sentry.io/4505120926138368";
					       o.Debug = false;
					       o.TracesSampleRate = 1.0;
					       o.IsGlobalModeEnabled = true;
				       }))
				{
					Internals.WriteLine("Sentry initialized");
					StartImpl();
				}
			else
				StartImpl();
		}

		private void StartImpl()
		{
			Internals.WriteLine("Mounted endpoints:");
			foreach (var endpoint in endpoints)
				Internals.WriteLine($"{endpoint.url}");

			listener.Start();
			Internals.WriteLine($"Listening for connections on {url}");

			var connectionHandler = ConnectionHandler();
			connectionHandler.GetAwaiter().GetResult();

			listener.Close();
		}
		
		private static async Task ConnectionHandler()
		{
			while (true)
				try
				{
					var ctx = await listener.GetContextAsync();

					var request = ctx.Request;
					var response = ctx.Response;

					Internals.PrintRequestInfo(request);

					// Check if path is mapped to any endpoint
					if (Equals(endpoints, Enumerable.Empty<Endpoint>()))
					{
						LLAPI.Utilities.Error.ErrorPageResponse(HttpStatus.NotImplemented, ref request, ref response);
						continue;
					}

					// Check if content is empty
					if (request.HttpMethod == Method.POST.ToString() && request.ContentLength64 == 0)
					{
						LLAPI.Utilities.Error.ErrorPageResponse(HttpStatus.BadRequest, ref request, ref response);
						Internals.WriteLine("Content body is empty: aborting");
						response.Close();
						continue;
					}

					// Endpoint handling
					foreach (var endpoint in endpoints)
					{
						// Split URLs into fragments
						string[] requestUrl = request.Url.AbsolutePath.Trim('/').Split("/");
						string[] endpointUrl = endpoint.url.Split("/");
						var urlMatched = true;
						Dictionary<string, string> parameters = new();

						// Check if requested URL matches static or dynamic endpoint
						for (var i = 0; i < endpointUrl.Length; i++)
						{
							if (string.Equals(endpointUrl[i], requestUrl[i]))
								continue;

							if (dynamicRegex.IsMatch(endpointUrl[i]))
							{
								parameters.Add(endpointUrl[i].Trim(':'), requestUrl[i]);
								continue;
							}

							if (string.Equals(endpointUrl[i], "{recursive}"))
								break;
							
							urlMatched = false;
							break;
						}

						if (urlMatched)
						{
							endpoint.Task(ref request, ref response, parameters);
							requestResolved = true;
							response.Close();
						}
					}

					// Throw 404 if result is not resolved by any of mounted endpoints
					if (!requestResolved)
						LLAPI.Utilities.Error.ErrorPageResponse(HttpStatus.NotFound, ref request, ref response);
				}
				catch (Exception ex)
				{
					SentryWrapper.CaptureException(ex);
				}
		}
	}
}