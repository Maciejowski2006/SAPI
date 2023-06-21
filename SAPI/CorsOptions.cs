
namespace SAPI
{
	public enum AccessControlAllowOrigin
	{
		All,
		ThisOrigin,
		Custom
	}
	public struct CorsOptions
	{
		public string AllowOrigin { get; set; }
		public bool AllowCredentials { get; set; }
		public uint MaxAge { get; set; }
		public string[] AllowHeaders { get; set; }
		
		public CorsOptions()
		{
			AllowOrigin = "*";
			AllowCredentials = false;
			MaxAge = 5;
			AllowHeaders = new string[] {};
		}
	}

	public class CorsBuilder
	{
		private CorsOptions cors = new();

		/// <summary>
		/// From what origin should endpoint be available from (All, ThisOrigin)
		/// </summary>
		public CorsBuilder AllowOrigin(AccessControlAllowOrigin allowOrigin)
		{
			switch (allowOrigin)
			{
				case AccessControlAllowOrigin.All:
				{
					cors.AllowOrigin = "*";
					break;
				}
				case AccessControlAllowOrigin.ThisOrigin:
				{
					cors.AllowOrigin = Config.ReadConfig().Url;
					break;
				}
			}

			return this;
		}
		
		/// <summary>
		/// From what origin should endpoint be available from (custom url)
		/// </summary>
		public CorsBuilder AllowOrigin(string origin)
		{
			cors.AllowOrigin = origin;
			return this;
		}

		/// <summary>
		/// Add to builder, if the request should have credentials
		/// </summary>
		public CorsBuilder AllowCredentials()
		{
			cors.AllowCredentials = true;
			return this;
		}

		public CorsBuilder AllowHeaders(string[] headers)
		{
			cors.AllowHeaders = headers;
			return this;
		}

		/// <summary>
		/// Max time that preflight request should be cached for
		/// </summary>
		/// <param name="age">Max time (in seconds)</param>
		/// <returns></returns>
		public CorsBuilder MaxAge(uint age)
		{
			cors.MaxAge = age;
			return this;
		}
		
		public CorsOptions Build() => cors;
	}
}