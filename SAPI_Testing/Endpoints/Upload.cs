using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints;

public class Upload : Endpoint
{
	public override string url { get; } = "upload";

	protected override void Post(ref Packet packet)
	{
		FileUpload.SaveFile(Environment.CurrentDirectory, FileUpload.FileNamingSchemes.GUID, ref packet);

		Error.ErrorPageResponse(HttpStatus.NotAcceptable, ref packet);
	}
}