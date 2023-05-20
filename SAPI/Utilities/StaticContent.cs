﻿using System.Net;
using SAPI.Internal;
using SAPI.LLAPI;

namespace SAPI.Utilities
{
	public class StaticContent
	{
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
		public static void FileResponse(string path, ref Packet packet)
		{
			if (File.Exists(path))
				try
				{
					using (var input = File.Open(path, FileMode.Open))
					{
						//Adding permanent http response headers
						string mime;
						packet.Response.ContentType = mimeTypeMappings.TryGetValue(Path.GetExtension(path), out mime)
							? mime
							: "application/octet-stream";

						packet.Response.ContentLength64 = input.Length;
						packet.Response.AddHeader("Date", DateTime.Now.ToString("r"));
						packet.Response.AddHeader("Last-Modified", File.GetLastWriteTime(path).ToString("r"));

						var buffer = new byte[1024 * 32];
						int nbytes;
						while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
							packet.Response.OutputStream.Write(buffer, 0, nbytes);

						input.Close();
						packet.Response.OutputStream.Flush();
					}

					packet.Response.StatusCode = (int)HttpStatusCode.OK;
				}
				catch (Exception e)
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
		public static void HostDirectory(string path, ref Packet packet)
		{
			try
			{
				List<string> filesInDir = Directory.GetFiles(path).ToList();

				foreach (string fileInDir in filesInDir)
				{
					string fileName = Path.GetFileName(fileInDir);

					if (fileName == packet.Paramters["file"])
					{
						FileResponse(fileInDir, ref packet);
						break;
					}
				}
			}
			catch (Exception e)
			{
				Internals.WriteLine($"Error: {e}");
				Error.ErrorPageResponse(HttpStatus.NotFound, ref packet);
			}
		}

		public static void HostDirectoryRecursively(string path, string url, ref Packet packet)
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
						FileResponse(file.rel, ref packet);
						break;
					}
			}
			catch (DirectoryNotFoundException e)
			{
				Internals.WriteLine($"File not found in directory. Error: {e}");
			}
			catch (Exception e)
			{
				SentryWrapper.CaptureException(e);
				Error.ErrorPageResponse(HttpStatus.NotFound, ref packet);
			}
		}
	}
}