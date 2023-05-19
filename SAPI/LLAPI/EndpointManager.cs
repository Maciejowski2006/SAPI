namespace SAPI.LLAPI
{
	internal static class EndpointManager
	{
		public static void FindAndMount(ref List<IEndpoint> endpoints)
		{
			Type type = typeof(IEndpoint);
			IEnumerable<Type> endpointClasses = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

			foreach (Type endpointClass in endpointClasses)
			{
				IEndpoint endpoint = (Activator.CreateInstance(endpointClass) as IEndpoint)!;
				endpoints.Add(endpoint);
			}
		}
	}
}