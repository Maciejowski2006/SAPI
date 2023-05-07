using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace SAPI.TestRunner;

public class TestSendJson
{
	record DataModel(int id, string message);
	
	[Fact]
	public async void SendJson_BodyShouldBe_TheSameAsPostedData()
	{
		using HttpClient client = new();
		DataModel data = new DataModel(1, "message");
		string sentJson = JsonConvert.SerializeObject(data);
		StringContent sentContent = new (sentJson, Encoding.UTF8, "application/json");
		HttpResponseMessage response = await client.PostAsync("http://localhost:8000/send-json", sentContent);
			
		StreamReader reader = new(await response.Content.ReadAsStreamAsync());
		string responseContent = await reader.ReadToEndAsync();
		DataModel output = JsonConvert.DeserializeObject<DataModel>(responseContent);

		output.id.Should().Be(data.id);
		output.message.Should().Be(data.message);
	}
}