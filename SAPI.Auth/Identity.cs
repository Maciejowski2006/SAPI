namespace SAPI.Auth
{
	public sealed class Identity
	{
		public int Id { get; private set; }

		/// <summary>
		/// Identifier is what your clients will use to authenticate (username, password). You can also define a username or email yourself, although it won't be used to authenticate
		/// </summary>
		public string? Identifier { get; set; }

		public string Password
		{
			get => hashedPassword;
			set => password = value;
		}

		private string password;
		private string hashedPassword;

		public bool Create()
		{
			hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
			return Database.Insert(this);
		}

		public bool Verify()
		{
			
			if (Database.GetPassword(Identifier, out string hashedPassword))
			{
				return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
			}
			return false;
		}
	}
}