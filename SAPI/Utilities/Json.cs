using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace SAPI.Utilities
{
	public class Json
	{
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
	}
}