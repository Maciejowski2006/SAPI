using System.Text;
using Newtonsoft.Json;

namespace SAPI.API.Utilities
{
	public static class Json
	{
		/// <summary>
		/// Responds to the client with JSON object. Execute at the end of the task.
		/// </summary>
		/// <param name="json">Add here your object to send with json</param>
		/// <param name="response">Response ref you got from server</param>
		public static void Response<T>(T json, ref Packet packet)
		{
			var serializedJson = JsonConvert.SerializeObject(json);
			var data = Encoding.UTF8.GetBytes(serializedJson);
			packet.Response.ContentEncoding = Encoding.UTF8;
			packet.Response.ContentType = "application/json";
			packet.Response.ContentLength64 = data.LongLength;
			packet.Response.StatusCode = 200;

			if (packet.Request.HttpMethod != "HEAD")
			{
				packet.Response.OutputStream.Write(data, 0, data.Length);
			}
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