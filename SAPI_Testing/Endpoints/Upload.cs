using System.Net;
using System.Text;
using SAPI.Endpoints;
using SAPI.Utilities;
using SAPI.Utilities.FileUpload;

namespace Testing.Endpoints;

public class Upload : IEndpoint
{
	public string url { get; } = "upload";
	public Method method { get; } = Method.POST;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		string s = FileUpload.SaveFile(ref request, Environment.CurrentDirectory, (f) =>
		{
			return "bruh" + FileUpload.DetermineFileExtension(f);
		});


		Utilities.Error(HttpStatus.NotAcceptable, ref response);
	}
}