using SAPI;
using SAPI.API.Utilities;

namespace Testing.Endpoints;

public class Upload : Endpoint
{
	public override string url { get; } = "upload";

	protected override void Post(ref Packet packet)
	{
		//string path = FileIO.SaveFile(Environment.CurrentDirectory, FileIO.FileNamingScheme.Timestamp, ref packet);
		
		string path = FileIO.SaveFile("C:\\", (e) =>
		{
			Console.WriteLine(e);
			return $"{Environment.CurrentDirectory}/test.png";
		}, ref packet);

		Console.WriteLine(path);
		Error.Page(HttpStatus.NotAcceptable, ref packet);
	}
}