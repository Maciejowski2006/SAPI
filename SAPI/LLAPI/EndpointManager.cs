using System.Reflection;

namespace SAPI.LLAPI
{
	internal static class EndpointManager
	{
		public static void FindAndMount(ref List<Endpoint> endpoints)
		{
			Type type = typeof(Endpoint);
			IEnumerable<Type> endpointClasses = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => type.IsAssignableFrom(p) && !p.IsAbstract);

			foreach (Type endpointClass in endpointClasses)
			{
				Endpoint endpoint = (Activator.CreateInstance(endpointClass) as Endpoint)!;

				endpoints.Add(endpoint);
			}
		}

		public static string GetDefinedMethods(Type endpoint)
		{
			string[] methods = { "Get", "Post", "Put", "Patch", "Delete" };
			List<string> definedMethods = new();

			foreach (string method in methods)
			{
				if (endpoint.GetMember(method, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length != 0)
					definedMethods.Add(method.ToUpper());
			}

			return definedMethods.Count > 0 ? $"{String.Join(", ", definedMethods)}, OPTIONS" : "OPTIONS";
		}
	}
}