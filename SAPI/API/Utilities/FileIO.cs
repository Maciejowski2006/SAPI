using System.Net;
using FileTypeChecker;
using FileTypeChecker.Abstracts;
using SAPI.LLAPI;
using LowLevelAPI = SAPI.LLAPI.Utilities.FileIO;

namespace SAPI.API.Utilities
{
	public static class FileIO
	{
		#region Output
		
		private static string fileName;
		
		public enum FileNamingScheme
		{
			GUID,
			Timestamp,
			DateTime
		}
		
		/// <summary>
		/// Saves file from request to specified location.
		/// </summary>
		/// <param name="path">Path to which file should be saved in</param>
		/// <param name="namingScheme">Naming scheme which file will follow</param>
		/// <param name="request">Pass from Task()</param>
		/// <returns>Path to file</returns>
		public static string SaveFile(string path, FileNamingScheme namingScheme, ref Packet packet)
		{
			string tempFile = LowLevelAPI.SaveFile(packet.Request.ContentEncoding, LowLevelAPI.GetBoundary(packet.Request.ContentType), packet.Request.InputStream);

			switch (namingScheme)
			{
				case FileNamingScheme.GUID:
				{
					fileName = Guid.NewGuid().ToString();
					fileName += DetermineFileExtension(tempFile);

					break;
				}
				case FileNamingScheme.Timestamp:
				{
					fileName = Convert.ToString((UInt64)DateTime.Now.AddMilliseconds(2).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
					fileName += DetermineFileExtension(tempFile);

					break;
				}
				case FileNamingScheme.DateTime:
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
		/// <param name="customFileNameHandler">Custom handler(method) for naming files: param -> temp file location; return -> new file name(with extension)</param>
		/// <param name="request">Pass from Task()</param>
		/// <returns>Path to file</returns>
		public static string SaveFile(string path, Func<string, string> customFileNameHandler, ref Packet packet)
		{
			string tempFile = LowLevelAPI.SaveFile(packet.Request.ContentEncoding, LowLevelAPI.GetBoundary(packet.Request.ContentType), packet.Request.InputStream);

			fileName = customFileNameHandler(tempFile);

			string finalPath = Path.Combine(path, fileName);

			File.Copy(tempFile, finalPath);
			File.Delete(tempFile);
			return finalPath;
		}
		
		/// <summary>
		/// Determines file extension based on it's magic bytes
		/// </summary>
		/// <param name="file">Path to file</param>
		/// <returns>File extension(without ".")</returns>
		public static string DetermineFileExtension(string file)
		{
			using FileStream fs = File.OpenRead(file);
			IFileType f = FileTypeValidator.GetFileType(fs);
			return f.Extension;
		}
		
		#endregion

		#region Input

		private static readonly IDictionary<string, string> mimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
		{
			{ ".asf", "video/x-ms-asf" },
			{ ".asx", "video/x-ms-asf" },
			{ ".avi", "video/x-msvideo" },
			{ ".bin", "application/octet-stream" },
			{ ".cco", "application/x-cocoa" },
			{ ".crt", "application/x-x509-ca-cert" },
			{ ".css", "text/css" },
			{ ".deb", "application/octet-stream" },
			{ ".der", "application/x-x509-ca-cert" },
			{ ".dll", "application/octet-stream" },
			{ ".dmg", "application/octet-stream" },
			{ ".ear", "application/java-archive" },
			{ ".exe", "application/octet-stream" },
			{ ".flv", "video/x-flv" },
			{ ".gif", "image/gif" },
			{ ".htm", "text/html" },
			{ ".html", "text/html" },
			{ ".ico", "image/x-icon" },
			{ ".img", "application/octet-stream" },
			{ ".iso", "application/octet-stream" },
			{ ".jar", "application/java-archive" },
			{ ".jardiff", "application/x-java-archive-diff" },
			{ ".jpeg", "image/jpeg" },
			{ ".jpg", "image/jpeg" },
			{ ".js", "application/x-javascript" },
			{ ".mov", "video/quicktime" },
			{ ".mp3", "audio/mpeg" },
			{ ".mp4", "video/mp4" },
			{ ".mpeg", "video/mpeg" },
			{ ".mpg", "video/mpeg" },
			{ ".msi", "application/octet-stream" },
			{ ".pdb", "application/x-pilot" },
			{ ".pdf", "application/pdf" },
			{ ".pem", "application/x-x509-ca-cert" },
			{ ".png", "image/png" },
			{ ".rar", "application/x-rar-compressed" },
			{ ".rpm", "application/x-redhat-package-manager" },
			{ ".run", "application/x-makeself" },
			{ ".shtml", "text/html" },
			{ ".swf", "application/x-shockwave-flash" },
			{ ".txt", "text/plain" },
			{ ".war", "application/java-archive" },
			{ ".xml", "text/xml" },
			{ ".xpi", "application/x-xpinstall" },
			{ ".zip", "application/zip" }
		};

		/// <summary>
		/// Responds to client with file
		/// </summary>
		/// <param name="path">Path to file</param>
		/// <param name="response">Pass from Task()</param>
		public static void ServeFile(string path, ref Packet packet)
		{
			if (File.Exists(path))
				try
				{
					using (var input = File.Open(path, FileMode.Open))
					{
						string file = Path.GetFileName(path);
						
						//Adding permanent http response headers
						packet.Response.ContentType = mimeTypeMappings.TryGetValue(Path.GetExtension(path), out string mime) ? mime : "application/octet-stream";

						packet.Response.ContentLength64 = input.Length;
						packet.Response.AddHeader("Date", DateTime.Now.ToString("r"));
						packet.Response.AddHeader("Last-Modified", File.GetLastWriteTime(path).ToString("r"));
						packet.Response.AddHeader("Content-Disposition", $"filename={file}");

						var buffer = new byte[1024 * 32];
						int nbytes;
						while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
							packet.Response.OutputStream.Write(buffer, 0, nbytes);

						input.Close();
						packet.Response.OutputStream.Flush();
					}

					packet.Response.StatusCode = (int)HttpStatusCode.OK;
				}
				catch
				{
					packet.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				}
			else
				packet.Response.StatusCode = (int)HttpStatusCode.NotFound;

			packet.Response.OutputStream.Close();
		}

		/// <summary>
		/// Exposes a directory contents to be accessed by clients
		/// </summary>
		/// <param name="path">Path to directory</param>
		/// <param name="parameters">Pass from Task()</param>
		/// <param name="response">Pass from Task()</param>
		public static void ServeDirectory(string path, ref Packet packet)
		{
			try
			{
				List<string> filesInDir = Directory.GetFiles(path).ToList();

				foreach (string fileInDir in filesInDir)
				{
					string fileName = Path.GetFileName(fileInDir);

					if (fileName == packet.Paramters["file"])
					{
						ServeFile(fileInDir, ref packet);
						break;
					}
				}
			}
			catch (Exception e)
			{
				Debug.Log($"Error: {e}");
				Error.Page(HttpStatus.NotFound, ref packet);
			}
		}

		public static void ServeDirectoryRecursively(string path, string url, ref Packet packet)
		{
			try
			{
				path = path.Replace('\\', '/');
				string recursivePath = packet.Request.Url.AbsolutePath.Substring(url.LastIndexOf('{') + 1);
				List<string> filesInDir = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
				List<string> absoluteFiles = new();

				foreach (var file in filesInDir)
					absoluteFiles.Add(file.Substring(path.Length).Trim('\\').Replace('\\', '/'));

				IEnumerable<(string rel, string abs)> zip = filesInDir.Zip(absoluteFiles);

				foreach ((string rel, string abs) file in zip)
					if (recursivePath == file.abs)
					{
						ServeFile(file.rel, ref packet);
						break;
					}
			}
			catch (DirectoryNotFoundException e)
			{
				Debug.Log($"File not found in directory. Error: {e}");
			}
			catch (Exception e)
			{
				SentryWrapper.CaptureException(e);
				Error.Page(HttpStatus.InternalServerError, ref packet);
			}
		}

		#endregion
	}
}