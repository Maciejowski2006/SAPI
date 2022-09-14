using System.Net;
namespace SAPI.Endpoints
{
	public enum Method
	{
		GET,
		POST,
		PUT,
		PATCH,
		DELETE,
		OPTIONS,
		HEAD
	}
	public abstract class Endpoint
    {
    	public string url;
        public Method method;

        public Endpoint(string url, Method method)
        {
	        this.url = url;
	        this.method = method;
        }

        public abstract void Task(ref HttpListenerRequest request, ref HttpListenerResponse response);
    }
}