using Newtonsoft.Json;
using System.Text;

namespace SAPI.TestRunner;

public class TestJson
{
	[Theory]
	[InlineData(1, "test")]
	[InlineData(5, "data")]
	[InlineData(14, "otherText")]
	public async void Json_Get_BodyShouldBe_TheSameAsPostedData(int inlineId, string InlineMessage)
	{
		Testing.Endpoints.DataModel model;
		
		using HttpClient client = new();
		Testing.Endpoints.DataModel data = new (inlineId, InlineMessage);
		string sentJson = JsonConvert.SerializeObject(data);
		StringContent sentContent = new (sentJson, Encoding.UTF8, "application/json");
		HttpResponseMessage response = await client.PostAsync("http://localhost:8000/json", sentContent);
			
		StreamReader reader = new(await response.Content.ReadAsStreamAsync());
		string responseContent = await reader.ReadToEndAsync();
		Testing.Endpoints.DataModel output = JsonConvert.DeserializeObject<Testing.Endpoints.DataModel>(responseContent);

		output.id.Should().Be(inlineId);
		output.message.Should().Be(InlineMessage);
	}
	
	[Fact]
	public async void Json_Get_BodyShouldBe_VerySpecific()
	{
		using HttpClient client = new();

		HttpResponseMessage response = await client.GetAsync("http://localhost:8000/json");
			
		StreamReader reader = new(await response.Content.ReadAsStreamAsync());
		string responseContent = await reader.ReadToEndAsync();
		Testing.Endpoints.DataModel output = JsonConvert.DeserializeObject<Testing.Endpoints.DataModel>(responseContent);

		output.id.Should().Be(14);
		output.message.Should().Be("Hello, World!");
	}
}