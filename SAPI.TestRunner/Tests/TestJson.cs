using Newtonsoft.Json;
using System.Text;

namespace SAPI.TestRunner;

public class TestJson
{
	public record DataModel(int id, string message);
	
	[Theory]
	[InlineData(1, "test")]
	[InlineData(5, "data")]
	[InlineData(14, "otherText")]
	public async void Json_Get_BodyShouldBe_TheSameAsPostedData(int inlineId, string InlineMessage)
	{
		using HttpClient client = new();
		DataModel data = new DataModel(inlineId, InlineMessage);
		string sentJson = JsonConvert.SerializeObject(data);
		StringContent sentContent = new (sentJson, Encoding.UTF8, "application/json");
		HttpResponseMessage response = await client.PostAsync("http://localhost:8000/json", sentContent);
			
		StreamReader reader = new(await response.Content.ReadAsStreamAsync());
		string responseContent = await reader.ReadToEndAsync();
		DataModel output = JsonConvert.DeserializeObject<DataModel>(responseContent);

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
		DataModel output = JsonConvert.DeserializeObject<DataModel>(responseContent);

		output.id.Should().Be(14);
		output.message.Should().Be("Hello, World!");
	}
}