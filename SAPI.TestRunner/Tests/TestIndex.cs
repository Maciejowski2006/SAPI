using System.Net;

namespace SAPI.TestRunner;

public class TestIndex
{
	[Fact]
	public async void Index_GET_ShouldBe_NotFound()
	{
		using HttpClient client = new();
		HttpResponseMessage res = await client.GetAsync("http://localhost:8000/");

		res.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}