﻿using System.Net;
using System.Text;
using FileTypeChecker.Abstracts;
using FileTypeChecker;

namespace SAPI.Utilities.FileUpload
{
	public class FileUpload
	{
		private static string fileName;
		private static string tempFile;

		public delegate string CustomFileNameHandler(string filePath);

		public enum FileNamingSchemes
		{
			GUID,
			Timestamp,
			DateTime,
			Custom
		}

		/// <summary>
		/// Saves file from request to specified location.
		/// </summary>
		/// <param name="path">Path to which file should be saved in</param>
		/// <param name="namingScheme">Naming scheme which file will follow</param>
		/// <param name="request">Pass from Task()</param>
		/// <returns>Path to file</returns>
		public static string SaveFile(string path, FileNamingSchemes namingScheme, ref HttpListenerRequest request)
		{
			SaveFileImpl(request.ContentEncoding, GetBoundary(request.ContentType), request.InputStream);

			switch (namingScheme)
			{
				case FileNamingSchemes.GUID:
				{
					fileName = Guid.NewGuid().ToString();
					fileName += DetermineFileExtension(tempFile);

					break;
				}
				case FileNamingSchemes.Timestamp:
				{
					fileName = Convert.ToString((UInt64)DateTime.Now.AddMilliseconds(2).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
					fileName += DetermineFileExtension(tempFile);

					break;

				}
				case FileNamingSchemes.DateTime:
				{
					DateTime dt = DateTime.Now.AddMilliseconds(2);

					fileName = $"{dt.Day}-{dt.Month}-{dt.Year}_{dt.Hour}.{dt.Minute}.{dt.Second}.{dt.Millisecond}";
					fileName += DetermineFileExtension(tempFile);

					break;
				}
			}

			string finalPath = Path.Combine(path, fileName);

			File.Copy(tempFile, finalPath);
			File.Delete(tempFile);
			return finalPath;
		}

		/// <summary>
		/// Saves file from request to specified location.
		/// </summary>
		/// <param name="path">Path to which file should be saved in</param>
		/// <param name="customFileNameHandler">Custom handler(method) for naming files -> return string(with extension)</param>
		/// <param name="request">Pass from Task()</param>
		/// <returns>Path to file</returns>
		public static string SaveFile(string path, CustomFileNameHandler customFileNameHandler, ref HttpListenerRequest request)
		{
			SaveFileImpl(request.ContentEncoding, GetBoundary(request.ContentType), request.InputStream);

			fileName = customFileNameHandler(tempFile);

			string finalPath = Path.Combine(path, fileName);

			File.Copy(tempFile, finalPath);
			File.Delete(tempFile);
			return finalPath;
		}

		/// <summary>
		/// Determines file extension based on it's header
		/// </summary>
		/// <param name="file">Path to file</param>
		/// <returns>File extension(with ".")</returns>
		public static string DetermineFileExtension(string file)
		{
			using (var fs = File.OpenRead(file))
			{
				IFileType f = FileTypeValidator.GetFileType(fs);
				string ext = $".{f.Extension}";
				return ext;
			}
		}

		private static void SaveFileImpl(Encoding enc, String boundary, Stream input)
		{
			Byte[] boundaryBytes = enc.GetBytes(boundary);
			Int32 boundaryLen = boundaryBytes.Length;

			tempFile = Path.GetTempFileName();

			using (FileStream output = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
			{
				Byte[] buffer = new Byte[1024];
				int len = input.Read(buffer, 0, 1024);
				int startPos = -1;

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
				for (int i = 0; i < 4; i++)
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
					int endPos = IndexOf(buffer, len, boundaryBytes);
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
		private static string GetBoundary(string type)
		{
			return "--" + type.Split(';')[1].Split('=')[1];
		}

		private static int IndexOf(Byte[] buffer, int len, Byte[] boundaryBytes)
		{
			for (Int32 i = 0; i <= len - boundaryBytes.Length; i++)
			{
				bool match = true;
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