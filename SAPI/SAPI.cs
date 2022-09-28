using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace SAPI
{
	public class Server
	{
		/// <summary>
		/// Initalizes SAPI on default address: http://localhost:8000/
		/// </summary>
		/// <param name="url">Sets custom url - remember to put "/" at the end. If no parameter is provided, SAPI starts on default address</param>
		public Server(string url = "http://localhost:8000/")
		{
			this.url = url;
			
			listener = new HttpListener();
			listener.Prefixes.Add(url);
			endpoints = new List<Endpoint>();
			iEndpoints = new();
		}

		private static HttpListener listener;
		private string url;
		private static int requestCount;
		private static List<Endpoint> endpoints;
		private static List<IEndpoint> iEndpoints;
		
		/// <summary>
		/// Starts the SAPI server. Execute at the end.
		/// </summary>
		public void Start()
		{
			if (!Equals(endpoints, Enumerable.Empty<Endpoint>()))
			{
				PrintDeprecationNotice();
				
				Console.WriteLine("Mounting endpoints...");
				foreach (Endpoint endpoint in endpoints)
					Console.WriteLine($"{endpoint.url} : {endpoint.method}");
				Console.Write("\n");
			}

			Console.WriteLine("Mounting interface endpoints...");
			foreach (IEndpoint endpoint in iEndpoints)
				Console.WriteLine($"{endpoint.url} : {endpoint.method}");
			
			Console.WriteLine("Done\n");
			
			listener.Start();
			Console.WriteLine($"Listening for connections on {url}");

			Task connectionHandler = ConnectionHandler();
			connectionHandler.GetAwaiter().GetResult();
			
			listener.Close();
		}
		public void MountEndpoint(Endpoint endpoint) => endpoints.Add(endpoint);
		public void MountEndpoint(IEndpoint endpoint) => iEndpoints.Add(endpoint);
		private static void PrintDeprecationNotice()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("WARNING: Standard endpoints are deprecated - they are going to be removed in the next major version. Please adapt your endpoints to interfaces. For help see documentation");
			Console.ResetColor();
		}

		private static void PrintRequestInfo(HttpListenerRequest request)
		{
			Console.WriteLine("Request #{0}", ++requestCount);
			Console.WriteLine($"URL: {request.Url}");
			Console.WriteLine($"Method: {request.HttpMethod}");
			Console.WriteLine($"User IPv4: {request.UserHostAddress}");
			Console.WriteLine($"User-Agent: {request.UserAgent}");
			Console.WriteLine("\n");
		}

		static async Task ConnectionHandler()
		{
			while (true)
			{
				HttpListenerContext ctx = await listener.GetContextAsync();

				HttpListenerRequest request = ctx.Request;
				HttpListenerResponse response = ctx.Response;

				PrintRequestInfo(request);

				bool requestResolved = false;

				if (Equals(endpoints, Enumerable.Empty<Endpoint>()) && Equals(iEndpoints, Enumerable.Empty<IEndpoint>()))
				{
					Utilities.Utilities.Error(HttpStatus.NotImplemented, ref response);
					continue;
				}

				// Old/Deprecated endpoint handling
				foreach (Endpoint endpoint in endpoints)
				{
					if (request.HttpMethod == endpoint.method.ToString())
					{
						if (request.Url.AbsolutePath == $"/{endpoint.url}")
						{
							endpoint.Task(ref request, ref response);
							response.StatusCode = 200;
							requestResolved = true;
							response.Close();
							PrintDeprecationNotice();
						}
					}
				}
				// New endpoint handling
				foreach (IEndpoint endpoint in iEndpoints)
				{
					if (request.HttpMethod == endpoint.method.ToString() && request.Url.AbsolutePath == $"/{endpoint.url}")
					{
						endpoint.Task(ref request, ref response);
						response.StatusCode = 200;
						requestResolved = true;
						response.Close();
					}
				}

				// Throw 501 if result is not resolved by any of mounted endpoints
				if (!requestResolved)
					Utilities.Utilities.Error(HttpStatus.NotFound, ref response);
			}
		}
	}
}