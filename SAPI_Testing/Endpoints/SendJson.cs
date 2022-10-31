using System.Net;
using SAPI.Endpoints;
using SAPI.Utilities;
namespace Testing.Endpoints;

internal record Model(string name, int age);
public class SendJson : IEndpoint
{

	public string url { get; } = "send-json";
	public Method method { get; } = Method.POST;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response)
	{
		Utilities.JsonFetch(out Model structure, ref request);

		Console.WriteLine($"Name: {structure.name} Age: {structure.age}");
	}
}
