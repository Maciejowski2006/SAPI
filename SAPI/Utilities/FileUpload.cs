using System;
using System.Net;
using System.Text;
using System;
using System.IO;
using FileTypeChecker.Abstracts;
using FileTypeChecker.Types;
using FileTypeChecker.Extensions;
using FileTypeChecker;
using static System.Net.Mime.MediaTypeNames;

namespace SAPI.Utilities.FileUpload
{
	// TODO: Documentation
	public class FileUpload
	{
		private static string fileName;
		private static string tempFile;

		public delegate string CustomFileNameHandler(string filePath);
		private static CustomFileNameHandler customFileNameHandler = null;


		public static string SaveFile(ref HttpListenerRequest request, string path, CustomFileNameHandler customFileNameHandler = null)
		{
			SaveFileImpl(request.ContentEncoding, GetBoundary(request.ContentType), request.InputStream);

			if (customFileNameHandler == null)
			{
				fileName = Guid.NewGuid().ToString();

				fileName += DetermineFileExtension(tempFile);
			}
			else
			{
				fileName = customFileNameHandler(tempFile);
			}

			string finalPath = Path.Combine(path, fileName);

			File.Copy(tempFile, finalPath);

			return finalPath;
		}

		public static string DetermineFileExtension(string file)
		{
			using (var fs = File.OpenRead(file))
			{
				IFileType f = FileTypeValidator.GetFileType(fs);
				string ext = $".{f.Extension}";
				return ext;
			}
		}


		private static string GetBoundary(string type)
		{
			return "--" + type.Split(';')[1].Split('=')[1];
		}

		private static void SaveFileImpl(Encoding enc, String boundary, Stream input)
		{
			Byte[] boundaryBytes = enc.GetBytes(boundary);
			Int32 boundaryLen = boundaryBytes.Length;

			tempFile = Path.GetTempFileName();

			using (FileStream output = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
			{
				Byte[] buffer = new Byte[1024];
				Int32 len = input.Read(buffer, 0, 1024);
				Int32 startPos = -1;

				// Find start boundary
				while (true)
				{
					if (len == 0)
					{
						throw new Exception("Start Boundaray Not Found");
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
}
