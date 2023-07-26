using SAPI;

// ReSharper disable CheckNamespace
namespace Tutorials.ServerBrowser
{
	public class ServerBrowser
	{
		public static void Main()
		{
			Server sapi = new();
			sapi.Start();
		}
	}
}