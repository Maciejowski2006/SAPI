using LiteDB;

namespace SAPI.Auth
{
	internal static class Database
	{
		private static string dbPath;
		private static ILiteCollection<Identity> userCollection;
		public static void Init()
		{
			dbPath = Path.Combine(Directory.GetCurrentDirectory(), "SAPI.Auth.db");
		}

		public static bool Insert(Identity identity)
		{
			using LiteDatabase db = new(dbPath);
			userCollection = db.GetCollection<Identity>("users");

			var results = userCollection.Query()
				.Where(x => x.Identifier == identity.Identifier).ToList();

			if (results.Count > 0)
				return false;
			
			userCollection.EnsureIndex(x => x.Identifier);
			userCollection.Insert(identity);

			return true;
		}

		public static bool GetPassword(string identifier, out string hashedPassword)
		{
			using LiteDatabase db = new(dbPath);
			userCollection = db.GetCollection<Identity>("users");
			var results = userCollection.Query()
				.Where(x => x.Identifier == identifier)
				.Select(x => x.Password)
				.ToList();
            
			if (results.Count > 0)
			{
				hashedPassword = results[0];
				return true;
			}

			hashedPassword = null;
			return false;
		}
	}
}