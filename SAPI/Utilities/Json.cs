using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace SAPI.Utilities
{
	public class Json
	{
		/// TODO: Remove this method after rewrite
		/// <summary>
		/// Responds to client with JSON object. Execute at the end of the task.
		/// </summary>
		/// <param name="json">Add here your object to send with json</param>
		/// <param name="response">Response ref you got from server - argument in Task()</param>
		public static void Response<T>(T json, ref HttpListenerResponse response)
		{
			var serializedJson = JsonConvert.SerializeObject(json);
			var data = Encoding.UTF8.GetBytes(serializedJson);
			response.ContentEncoding = Encoding.UTF8;
			response.ContentType = "application/json";
			response.ContentLength64 = data.LongLength;
			response.StatusCode = 200;

			response.OutputStream.Write(data, 0, data.Length);
		}

		/// TODO: Remove this method after rewrite
		/// <summary>
		/// Fetches structured JSON data from request. Execute before trying to access given data
		/// </summary>
		/// <param name="json">Add here your instance of an object to be serialized</param>
		/// <param name="request">Request ref you got from server - argument in Task()</param>
		public static void Fetch<T>(out T json, ref HttpListenerRequest request)
		{
			StreamReader reader = new(request.InputStream, request.ContentEncoding);

			var data = reader.ReadToEnd();
			json = JsonConvert.DeserializeObject<T>(data);

			reader.Close();
		}
		
		/// <summary>
		/// Responds to client with JSON object. Execute at the end of the task.
		/// </summary>
		/// <param name="json">Add here your object to send with json</param>
		/// <param name="response">Response ref you got from server - argument in Task()</param>
		public static void Response<T>(T json, ref Packet packet)
		{
			var serializedJson = JsonConvert.SerializeObject(json);
			var data = Encoding.UTF8.GetBytes(serializedJson);
			packet.Response.ContentEncoding = Encoding.UTF8;
			packet.Response.ContentType = "application/json";
			packet.Response.ContentLength64 = data.LongLength;
			packet.Response.StatusCode = 200;

			packet.Response.OutputStream.Write(data, 0, data.Length);
		}

		/// <summary>
		/// Fetches structured JSON data from request. Execute before trying to access given data
		/// </summary>
		/// <param name="json">Add here your instance of an object to be serialized</param>
		/// <param name="request">Request ref you got from server - argument in Task()</param>
		public static void Fetch<T>(out T json, ref Packet packet)
		{
			StreamReader reader = new(packet.Request.InputStream, packet.Request.ContentEncoding);

			var data = reader.ReadToEnd();
			json = JsonConvert.DeserializeObject<T>(data);

			reader.Close();
		}
	}
}