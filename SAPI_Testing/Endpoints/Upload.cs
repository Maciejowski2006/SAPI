using System.Net;
using System.Text;
using SAPI.Endpoints;
using SAPI.Utilities;

namespace Testing.Endpoints;

public class Upload : IEndpoint
{
	public string url { get; } = "upload";
	public Method method { get; } = Method.POST;
	public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
	{
		SaveFile(request.ContentEncoding, GetBoundary(request.ContentType), request.Headers.Get("File-Name"), request.InputStream);

		StreamReader r = new(request.InputStream);
		Console.WriteLine(r.ReadToEnd());

		Utilities.Error(HttpStatus.NotAcceptable, ref response);
	}

	private static String GetBoundary(String ctype)
	{
		return "--" + ctype.Split(';')[1].Split('=')[1];
	}

	private static void SaveFile(Encoding enc, String boundary, string name, Stream input)
	{
		Byte[] boundaryBytes = enc.GetBytes(boundary);
		Int32 boundaryLen = boundaryBytes.Length;


		using (FileStream output = new FileStream(name, FileMode.Create, FileAccess.Write))
		{
			Byte[] buffer = new Byte[1024];
			Int32 len = input.Read(buffer, 0, 1024);
			Int32 startPos = -1;

			// Find start boundary
			while (true)
			{
				if (len == 0)
				{
					throw new Exception("Start Boundary Not Found");
				}

				startPos = IndexOf(buffer, len, boundaryBytes);
				if (startPos >= 0)
				{
					break;
				}
				else
				{
					Array.Copy(buffer, len - boundaryLen, buffer, 0, boundaryLen);
					len = input.Read(buffer, boundaryLen, 1024 - boundaryLen);
				}
			}

			// Skip four lines (Boundary, Content-Disposition, Content-Type, and a blank)
			for (Int32 i = 0; i < 4; i++)
			{
				while (true)
				{
					if (len == 0)
					{
						throw new Exception("Preamble not Found.");
					}

					startPos = Array.IndexOf(buffer, enc.GetBytes("\n")[0], startPos);
					if (startPos >= 0)
					{
						startPos++;
						break;
					}
					else
					{
						len = input.Read(buffer, 0, 1024);
					}
				}
			}

			Array.Copy(buffer, startPos, buffer, 0, len - startPos);
			len = len - startPos;

			while (true)
			{
				Int32 endPos = IndexOf(buffer, len, boundaryBytes);
				if (endPos >= 0)
				{
					if (endPos > 0) output.Write(buffer, 0, endPos - 2);
					break;
				}
				else if (len <= boundaryLen)
				{
					throw new Exception("End Boundary Not Found");
				}
				else
				{
					output.Write(buffer, 0, len - boundaryLen);
					Array.Copy(buffer, len - boundaryLen, buffer, 0, boundaryLen);
					len = input.Read(buffer, boundaryLen, 1024 - boundaryLen) + boundaryLen;
				}
			}
		}
	}

	private static Int32 IndexOf(Byte[] buffer, Int32 len, Byte[] boundaryBytes)
	{
		for (Int32 i = 0; i <= len - boundaryBytes.Length; i++)
		{
			Boolean match = true;
			for (Int32 j = 0; j < boundaryBytes.Length && match; j++)
			{
				match = buffer[i + j] == boundaryBytes[j];
			}

			if (match)
			{
				return i;
			}
		}

		return -1;
	}
}