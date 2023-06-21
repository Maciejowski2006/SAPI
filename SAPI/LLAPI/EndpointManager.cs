
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
	}
}