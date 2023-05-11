using Sentry;

namespace SAPI.LLAPI
{
	internal class SentryWrapper
	{
		public static void CaptureException(Exception e)
		{
			if (Config.ReadConfig().EnableErrorReporting)
				SentrySdk.CaptureException(e);

			Console.WriteLine($"Exception: {e}");
		}
	}
}