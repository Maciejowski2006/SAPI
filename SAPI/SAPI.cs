using System.Net;
using System.Text.RegularExpressions;
using SAPI.API.Utilities;
using SAPI.LLAPI;
using Sentry;
using Debug = SAPI.LLAPI.Debug;

namespace SAPI
{
	public class Server
	{
		private static HttpListener listener;
		private string url;
		private static List<Endpoint> endpoints;
		private static List<IExtensionBase> extensions = new();
		private static Regex dynamicRegex = new(":(.+?)(?:(?=/)|$)", RegexOptions.Compiled);

		/// <summary>
		/// Initializes SAPI on default address: http://localhost:8000/
		/// </summary>
		public Server()
		{
			Debug.Init();

			Config.Init();
			url = Config.ReadConfig().Url;

			listener = new HttpListener();
			listener.Prefixes.Add(url);
			endpoints = new List<Endpoint>();
			EndpointManager.FindAndMount(ref endpoints);
		}

		public Server Use(IExtensionBase extension)
		{
			extensions.Add(extension);
			return this;
		}
		
		/// <summary>
		/// Starts the SAPI server. Execute at the end.
		/// </summary>
		public void Start()
		{
			// Skip Sentry if in Debug mode
			#if DEBUG
			StartImpl();
			return;
			#endif
			if (Config.ReadConfig().EnableErrorReporting)
				using (SentrySdk.Init(o =>
				       {
					       o.Dsn = "https://024ccd09f43a47e889fbb78388d7b916@o1346432.ingest.sentry.io/4505120926138368";
					       o.Debug = false;
					       o.TracesSampleRate = 1.0;
					       o.IsGlobalModeEnabled = true;
				       }))
				{
					Debug.Log("Sentry initialized");
					StartImpl();
				}
			else
				StartImpl();
		}

		private void StartImpl()
		{
			foreach (IExtensionBase extension in extensions)
				extension.Init();
			
			Debug.Log("Mounted endpoints:");
			foreach (Endpoint endpoint in endpoints)
				Debug.Log($"{endpoint.url}");

			listener.Start();
			Debug.Log($"Listening for connections on {url}");

			Task connectionHandler = ConnectionHandler();
			connectionHandler.GetAwaiter().GetResult();

			listener.Close();
		}

		private static async Task ConnectionHandler()
		{
			while (true)
				try
				{
					HttpListenerContext context = await listener.GetContextAsync();

					HttpListenerRequest request = context.Request;
					HttpListenerResponse response = context.Response;

					Internals.PrintRequestInfo(request);

					// Check if content is empty
					if (request.HttpMethod == Method.POST.ToString() && request.ContentLength64 == 0)
					{
						Error.Page(HttpStatus.BadRequest, context);
						Debug.Warn("Content body is empty: aborting");
						response.Close();
						continue;
					}

					bool requestResolved = false;
					
					// Endpoint handling
					foreach (Endpoint endpoint in endpoints)
					{
						// Split URLs into fragments
						string[] requestUrl = request.Url.AbsolutePath.Trim('/').Split("/");
						string[] endpointUrl = endpoint.url.Split("/");
						bool urlMatched = true;
						Dictionary<string, string> parameters = new();

						// Check if requested URL matches static or dynamic endpoint
						for (int i = 0; i < endpointUrl.Length; i++)
						{
							if (requestUrl.Length < endpointUrl.Length)
							{
								urlMatched = false;
								break;
							}
							
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
							
							response.AddHeader("Server", "SAPI");
							endpoint.Task(context, parameters);
							requestResolved = true;
							response.Close();
						}
					}

					// Throw 404 if result is not resolved by any of mounted endpoints
					if (!requestResolved)
						Error.Page(HttpStatus.NotFound, context);
				}
				catch (Exception ex)
				{
					SentryWrapper.CaptureException(ex);
				}
		}
	}
}