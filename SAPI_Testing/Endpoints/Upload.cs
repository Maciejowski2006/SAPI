using System.Net;
using System.Text;
using SAPI;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class Upload : IEndpoint
{
	public string url { get; } = "upload";
	public Method method { get; } = Method.POST;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		FileUpload.SaveFile(Environment.CurrentDirectory, FileUpload.FileNamingSchemes.GUID, ref request);

		Error.ErrorPageResponse(HttpStatus.NotAcceptable, ref response);
	}
}