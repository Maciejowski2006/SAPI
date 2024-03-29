﻿using System.Reflection;

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

		public static bool CheckForDefinedMethod(string method, Type endpoint)
		{
			try
			{
				if (method == "Head" && CheckForDefinedMethod("Get", endpoint))
					return true;
				
				return endpoint.GetMember(method, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length != 0;
			}
			catch
			{
				return false;
			}
		}
		
		public static string GetDefinedMethods(Type endpoint)
		{
			string[] methods = { "Get", "Post", "Put", "Patch", "Delete", "Head" };
			List<string> definedMethods = new();

			foreach (string method in methods)
			{
				if (CheckForDefinedMethod(method, endpoint))
					definedMethods.Add(method.ToUpper());
			}

			return definedMethods.Count > 0 ? $"{String.Join(", ", definedMethods)}, OPTIONS" : "OPTIONS";
		}
	}
}