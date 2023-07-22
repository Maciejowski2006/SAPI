using System.Net;
using System.Runtime.InteropServices.ComTypes;
using SAPI;
using SAPI.API.Utilities;
using Tutorials.ServerBrowser.Services;

// ReSharper disable CheckNamespace
namespace Tutorials.ServerBrowser.Endpoints
{
	public class AddServer : Endpoint
	{
		public override string url { get; } = "add-server";

		protected override void Post(ref Packet packet)
		{
			if (Cookies.CheckForCookie("banned", out _, ref packet))
			{
				Error.Page(HttpStatus.NotAcceptable, ref packet);
				return;
			}

			List<string> apiKeys = Database.GetApiKeys();
			if (Auth.CheckForApiKey(apiKeys, ref packet))
			{
				Json.Fetch(out Models.Server server, ref packet);
				if (server.Name.ToLower().Contains("e-girl"))
				{
					Cookies.GiveCookie(new Cookie("banned", "true"), ref packet);
					Error.Page(HttpStatus.NotAcceptable, ref packet);
					return;
				}

				Database.AddServer(server);
			}
			else
			{
				Error.Page(HttpStatus.Unauthorized, ref packet);
			}
		}
	}
}