using SAPI;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class Upload : IEndpoint
{
	public string url { get; } = "upload";
	public void Post(ref Packet packet)
	{
		FileUpload.SaveFile(Environment.CurrentDirectory, FileUpload.FileNamingSchemes.GUID, ref packet);

		Error.ErrorPageResponse(HttpStatus.NotAcceptable, ref packet);
	}
}