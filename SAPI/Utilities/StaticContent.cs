using System.Net;
using Sentry;

namespace SAPI.Utilities.StaticContent
{
	public class StaticContent
	{
		public static IDictionary<string, string> mimeTypeMappings { get; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
		{
			{".asf", "video/x-ms-asf"},
			{".asx", "video/x-ms-asf"},
			{".avi", "video/x-msvideo"},
			{".bin", "application/octet-stream"},
			{".cco", "application/x-cocoa"},
			{".crt", "application/x-x509-ca-cert"},
			{".css", "text/css"},
			{".deb", "application/octet-stream"},
			{".der", "application/x-x509-ca-cert"},
			{".dll", "application/octet-stream"},
			{".dmg", "application/octet-stream"},
			{".ear", "application/java-archive"},
			{".exe", "application/octet-stream"},
			{".flv", "video/x-flv"},
			{".gif", "image/gif"},
			{".htm", "text/html"},
			{".html", "text/html"},
			{".ico", "image/x-icon"},
			{".img", "application/octet-stream"},
			{".iso", "application/octet-stream"},
			{".jar", "application/java-archive"},
			{".jardiff", "application/x-java-archive-diff"},
			{".jpeg", "image/jpeg"},
			{".jpg", "image/jpeg"},
			{".js", "application/x-javascript"},
			{".mov", "video/quicktime"},
			{".mp3", "audio/mpeg"},
			{".mp4", "video/mp4"},
			{".mpeg", "video/mpeg"},
			{".mpg", "video/mpeg"},
			{".msi", "application/octet-stream"},
			{".pdb", "application/x-pilot"},
			{".pdf", "application/pdf"},
			{".pem", "application/x-x509-ca-cert"},
			{".png", "image/png"},
			{".rar", "application/x-rar-compressed"},
			{".rpm", "application/x-redhat-package-manager"},
			{".run", "application/x-makeself"},
			{".shtml", "text/html"},
			{".swf", "application/x-shockwave-flash"},
			{".txt", "text/plain"},
			{".war", "application/java-archive"},
			{".xml", "text/xml"},
			{".xpi", "application/x-xpinstall"},
			{".zip", "application/zip"},
		};
		
		public static void FileResponse(string path, ref HttpListenerResponse response)
		{
			if (File.Exists(path))
			{
				try
				{
					using (FileStream input = File.Open(path, FileMode.Open))
					{
						//Adding permanent http response headers
						string mime;
						response.ContentType = mimeTypeMappings.TryGetValue(Path.GetExtension(path), out mime)
							? mime
							: "application/octet-stream";

						response.ContentLength64 = input.Length;
						response.AddHeader("Date", DateTime.Now.ToString("r"));
						response.AddHeader("Last-Modified", File.GetLastWriteTime(path).ToString("r"));

						byte[] buffer = new byte[1024 * 32];
						int nbytes;
						while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
							response.OutputStream.Write(buffer, 0, nbytes);
						
						input.Close();
						response.OutputStream.Flush();
					}
					
					response.StatusCode = (int) HttpStatusCode.OK;
				}
				catch (Exception e)
				{
					response.StatusCode = (int) HttpStatusCode.InternalServerError;
				}
			}
			else
				response.StatusCode = (int) HttpStatusCode.NotFound;

			response.OutputStream.Close();
		}

		public static void HostDirectory(string path, ref HttpListenerResponse response, Dictionary<string, string> parameters)
		{
			try
			{
				List<string> filesInDir = Directory.GetFiles(path).ToList();

				foreach (string fileInDir in filesInDir)
				{
					string fileName = Path.GetFileName(fileInDir);

					if (fileName == parameters["file"])
					{
						FileResponse(fileInDir, ref response);
						break;
					}
				}
			}
			catch (Exception e)
			{
				Internals.WriteLine($"Error: {e}");
				Utilities.Error(HttpStatus.NotFound, ref response);
			}
		}
		public static void HostDirectoryRecursively(string path, string url, ref HttpListenerRequest request, ref HttpListenerResponse response)
		{
			try
			{
				path = path.Replace('\\', '/');
				string recursivePath = request.Url.AbsolutePath.Substring(url.LastIndexOf('{') + 1);
				List<string> filesInDir = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
				List<string> absoluteFiles = new();
				
				foreach (string file in filesInDir)
					absoluteFiles.Add(file.Substring(path.Length).Trim('\\').Replace('\\', '/'));
				
				IEnumerable<(string rel, string abs)> zip = filesInDir.Zip(absoluteFiles);

				foreach ((string rel, string abs) file in zip)
				{
					if (recursivePath == file.abs)
					{
						FileResponse(file.rel, ref response);
						break;
					}
				}
			}
			catch (DirectoryNotFoundException e)
			{
				Internals.WriteLine($"File not found in directory. Error: {e}");
			}
			catch (Exception e)
			{
				SentryWrapper.CaptureException(e);
				Utilities.Error(HttpStatus.NotFound, ref response);
			}
		}
	}
}
