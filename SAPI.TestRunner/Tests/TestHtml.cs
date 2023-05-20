using System.Net;

namespace SAPI.TestRunner
{
	public class TestHtml
	{
		[Fact]
		public async void Html_Get_ShouldBe_OK()
		{
			using HttpClient client = new();

			HttpResponseMessage response = await client.GetAsync("http://localhost:8000/html");

			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}
		[Fact]
		public async void Html_Options_ShouldHave_CorsHeaders()
		{
			using HttpClient client = new();

			HttpResponseMessage response = await client.GetAsync("http://localhost:8000/html");
		}
	}
}