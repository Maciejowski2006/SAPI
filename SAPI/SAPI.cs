using System.Net;
using System.Text;
using SAPI.Endpoints;

namespace SAPI
{
	public class Server
	{

		public Server(string url)
		{
			this.url = url;
		}

		
		private static HttpListener listener;
		private string url = "http://localhost:8000";
		private static int requestCount;
		private static List<Endpoint> endpoints;
		public void Init()
		{
			listener = new HttpListener();
			listener.Prefixes.Add(url);
			endpoints = new List<Endpoint>();
		}
		public void Start()
		{
			Console.WriteLine("Mounting endpoints...");
			foreach (Endpoint endpoint in endpoints)
			{
				Console.WriteLine($"{endpoint.url} : {endpoint.method}");
			}
			Console.WriteLine("Done\n");
			
			listener.Start();
			Console.WriteLine($"Listening for connections on {url}");

			Task connectionHandler = ConnectionHandler();
			connectionHandler.GetAwaiter().GetResult();
			
			listener.Close();
		}
		public void MountEndpoint(Endpoint endpoint)
		{
			endpoints.Add(endpoint);
		}

		private static void PrintRequestInfo(HttpListenerRequest request)
		{
			Console.WriteLine("Request #{0}", ++requestCount);
			Console.WriteLine($"URL: {request.Url}");
			Console.WriteLine($"Method: {request.HttpMethod}");
			Console.WriteLine($"User IPv4: {request.UserHostAddress}");
			Console.WriteLine($"User-Agent: {request.UserAgent}");
		}

		public static async Task ConnectionHandler()
		{
			while (true)
			{
				HttpListenerContext ctx = await listener.GetContextAsync();

				HttpListenerRequest request = ctx.Request;
				HttpListenerResponse response = ctx.Response;

				PrintRequestInfo(request);

				bool requestResolved = false;
				
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
						}
					}
				}
				// Throw 501 if result is not resolved by any of mounted endpoints
				if (!requestResolved)
				{
					response.StatusCode = 501;
					response.StatusDescription = "API endpoint not implemented, or you are not supposed to be here";
					byte[] data = Encoding.UTF8.GetBytes("API endpoint not implemented, or you are not supposed to be here");
					response.ContentType = "text/html";
					response.ContentEncoding = Encoding.UTF8;
					response.ContentLength64 = data.LongLength;

					await response.OutputStream.WriteAsync(data, 0, data.Length);
				}
			}
		}
		
	}
}