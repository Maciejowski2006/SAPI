using System.Net;
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
		public static void Response<T>(T json, HttpListenerContext context)
		{
			var serializedJson = JsonConvert.SerializeObject(json);
			var data = Encoding.UTF8.GetBytes(serializedJson);
			context.Response.ContentEncoding = Encoding.UTF8;
			context.Response.ContentType = "application/json";
			context.Response.ContentLength64 = data.LongLength;
			context.Response.StatusCode = 200;

			if (context.Request.HttpMethod != "HEAD")
			{
				context.Response.OutputStream.Write(data, 0, data.Length);
			}
		}

		/// <summary>
		/// Fetches structured JSON data from request. Execute before trying to access given data
		/// </summary>
		/// <param name="json">Add here your instance of an object to be serialized</param>
		/// <param name="request">Request ref you got from server - argument in Task()</param>
		public static void Fetch<T>(out T json, HttpListenerContext context)
		{
			StreamReader reader = new(context.Request.InputStream, context.Request.ContentEncoding);

			var data = reader.ReadToEnd();
			json = JsonConvert.DeserializeObject<T>(data);

			reader.Close();
		}
	}
}