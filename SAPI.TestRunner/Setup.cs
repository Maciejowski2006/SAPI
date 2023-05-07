using System.Net;

namespace SAPI.TestRunner
{
	public class Setup : IDisposable
	{
		private Thread server;
		
		public Setup()
		{
			server = new Thread(SAPI_Testing.Main);
			server.Start();
		}
		public void Dispose() => server.Interrupt();
		
		[Fact]
		public async void Server_ShouldBe_Running()
		{
			using HttpClient client = new();
			try
			{
				await client.GetAsync("http://localhost:8000");
			}
			catch (HttpRequestException e)
			{
				Assert.Fail("Server is not running");
			}
		}
	}
}