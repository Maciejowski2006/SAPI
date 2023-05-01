using System.Net;
using System.Text.RegularExpressions;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace SAPI;

public class Server
{
	private static HttpListener listener;
	private string url;
	private static bool requestResolved;
	private static List<IEndpoint> endpoints;
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
		listener.Prefixes.Add(this.url);
		endpoints = new List<IEndpoint>();
	}

	/// <summary>
	/// Starts the SAPI server. Execute at the end.
	/// </summary>
	public void Start()
	{
		Internals.WriteLine("Mounted endpoints:");
		foreach (IEndpoint endpoint in endpoints)
			Internals.WriteLine($"{endpoint.method} {endpoint.url}");

		listener.Start();
		Internals.WriteLine($"Listening for connections on {url}");

		Task connectionHandler = ConnectionHandler();
		connectionHandler.GetAwaiter().GetResult();

		listener.Close();
	}

	public void MountEndpoint(IEndpoint endpoint) => endpoints.Add(endpoint);

	private static async Task ConnectionHandler()
	{
		while (true)
		{
			HttpListenerContext ctx = await listener.GetContextAsync();

			HttpListenerRequest request = ctx.Request;
			HttpListenerResponse response = ctx.Response;

			Internals.PrintRequestInfo(request);

			// Check if path is mapped to any endpoint
			if (Equals(endpoints, Enumerable.Empty<IEndpoint>()))
			{
				Utilities.Utilities.Error(HttpStatus.NotImplemented, ref response);
				continue;
			}

			// Check if content is empty
			if (request.HttpMethod == Method.POST.ToString() && request.ContentLength64 == 0)
			{
				Utilities.Utilities.Error(HttpStatus.BadRequest, ref response);
				Internals.WriteLine("Content body is empty: aborting");
				response.Close();
				continue;
			}

			// Endpoint handling
			foreach (IEndpoint endpoint in endpoints)
			{
				if (request.HttpMethod != endpoint.method.ToString())
					continue;

				// Split URLs into fragments
				string[] requestUrl = request.Url.AbsolutePath.Trim('/').Split("/");
				string[] endpointUrl = endpoint.url.Split("/");
				bool urlMatched = true;
				Dictionary<string, string> parameters = new();
				
				// Check if requested URL matches static or dynamic endpoint
				for (int i = 0; i < endpointUrl.Length; i++)
				{
					if (String.Equals(endpointUrl[i], requestUrl[i]))
						continue;
					
					if (dynamicRegex.IsMatch(endpointUrl[i]))
					{
						parameters.Add(endpointUrl[i].Trim(':'), requestUrl[i]);
						continue;
					}

					if (String.Equals(endpointUrl[i], "{recursive}"))
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
				Utilities.Utilities.Error(HttpStatus.NotFound, ref response);
		}
	}
}