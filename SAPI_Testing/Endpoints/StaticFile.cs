using System.Net;
using SAPI.Endpoints;

namespace Testing.Endpoints
{
	public class StaticFile : IEndpoint
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
		{".eot", "application/octet-stream"},
		{".exe", "application/octet-stream"},
		{".flv", "video/x-flv"},
		{".gif", "image/gif"},
		{".hqx", "application/mac-binhex40"},
		{".htc", "text/x-component"},
		{".htm", "text/html"},
		{".html", "text/html"},
		{".ico", "image/x-icon"},
		{".img", "application/octet-stream"},
		{".iso", "application/octet-stream"},
		{".jar", "application/java-archive"},
		{".jardiff", "application/x-java-archive-diff"},
		{".jng", "image/x-jng"},
		{".jnlp", "application/x-java-jnlp-file"},
		{".jpeg", "image/jpeg"},
		{".jpg", "image/jpeg"},
		{".js", "application/x-javascript"},
		{".mml", "text/mathml"},
		{".mng", "video/x-mng"},
		{".mov", "video/quicktime"},
		{".mp3", "audio/mpeg"},
		{".mp4", "video/mp4"},
		{".mpeg", "video/mpeg"},
		{".mpg", "video/mpeg"},
		{".msi", "application/octet-stream"},
		{".msm", "application/octet-stream"},
		{".msp", "application/octet-stream"},
		{".pdb", "application/x-pilot"},
		{".pdf", "application/pdf"},
		{".pem", "application/x-x509-ca-cert"},
		{".pl", "application/x-perl"},
		{".pm", "application/x-perl"},
		{".png", "image/png"},
		{".prc", "application/x-pilot"},
		{".ra", "audio/x-realaudio"},
		{".rar", "application/x-rar-compressed"},
		{".rpm", "application/x-redhat-package-manager"},
		{".rss", "text/xml"},
		{".run", "application/x-makeself"},
		{".sea", "application/x-sea"},
		{".shtml", "text/html"},
		{".sit", "application/x-stuffit"},
		{".swf", "application/x-shockwave-flash"},
		{".tcl", "application/x-tcl"},
		{".tk", "application/x-tcl"},
		{".txt", "text/plain"},
		{".war", "application/java-archive"},
		{".wbmp", "image/vnd.wap.wbmp"},
		{".wmv", "video/x-ms-wmv"},
		{".xml", "text/xml"},
		{".xpi", "application/x-xpinstall"},
		{".zip", "application/zip"},
	};
	
		
		public string url { get; } = "filetest";
		public Method method { get; } = Method.GET;
		public void Task(ref HttpListenerRequest request, ref HttpListenerResponse response, Dictionary<string, string> parameters)
		{
			string file = Path.Combine(Directory.GetCurrentDirectory(), "vid.mp4");

			Console.WriteLine(file);
			Console.WriteLine(Path.GetExtension(file));
			if (File.Exists(file))
			{
				try
				{
					Stream input = new FileStream(file, FileMode.Open);

					//Adding permanent http response headers
					string mime;
					response.ContentType = mimeTypeMappings.TryGetValue(Path.GetExtension(file), out mime)
						? mime
						: "application/octet-stream";
					response.ContentLength64 = input.Length;
					response.AddHeader("Date", DateTime.Now.ToString("r"));
					response.AddHeader("Last-Modified", File.GetLastWriteTime(file).ToString("r"));

					byte[] buffer = new byte[1024*32];
					int nbytes;
					while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
						response.OutputStream.Write(buffer, 0, nbytes);
					input.Close();
					response.OutputStream.Flush();

					response.StatusCode = (int) HttpStatusCode.OK;
				}
				catch (Exception ex)
				{
					response.StatusCode = (int) HttpStatusCode.InternalServerError;
				}
			}
			else
			{
				response.StatusCode = (int) HttpStatusCode.NotFound;
			}

			response.OutputStream.Close();
		}
	}
}
