namespace SAPI.LLAPI
{
	internal static class EndpointManager
	{
		// Only works for IEndpoint interface
		// public static void FindAndMount(ref List<IEndpoint> endpoints)
		// {
		// 	Type type = typeof(IEndpoint);
		// 	IEnumerable<Type> endpointClasses = AppDomain.CurrentDomain.GetAssemblies()
		// 		.SelectMany(s => s.GetTypes())
		// 		.Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
		//
		// 	foreach (Type endpointClass in endpointClasses)
		// 	{
		// 		IEndpoint endpoint = (Activator.CreateInstance(endpointClass) as IEndpoint)!;
		// 		endpoints.Add(endpoint);
		// 	}
		// }
		
		// For Endpoint abstract class
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