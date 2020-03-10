using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReduxSharp.Store.Attributes;

namespace ReduxSharp.Store.Selectors
{
	internal static class SelectorFactory
	{
		internal static Dictionary<Type, object> CreateSelectors(Assembly assembly)
		{
			var selectors = new Dictionary<Type, object>();
			var curAssembly = Assembly.GetExecutingAssembly();
			var funcTypes = Assembly.GetAssembly(typeof(Func<>)).ExportedTypes.Where(t => t.Name.StartsWith("Func"))
				.OrderBy(t => t.GenericTypeArguments.Length).ToList();
			var queryTypes = assembly.DefinedTypes.Where(type => type.GetCustomAttribute<QueryAttribute>() != null);
			var selectorTypes = curAssembly.DefinedTypes.Where(t => t.Name.StartsWith("Selector") && t.IsNotPublic).ToList();

			foreach (var type in queryTypes)
			{
				var attr = type.GetCustomAttribute<QueryAttribute>();
				var stateType = attr.MarkerType;
				var methods = type.DeclaredMethods.Where(m => m.IsPublic && m.IsStatic).ToDictionary(m => m.Name, m => m);

				foreach (var method in methods)
				{
					List<Type> genericTypes = new List<Type> { stateType };
					List<Type> inputTypes = new List<Type>();
					object[] args;
					var mAttr = method.Value.GetCustomAttribute<QueryAttribute>();
					if (mAttr.Dependencies.Length == 0)
					{
						inputTypes.Add(stateType);
						args = new object[] { };
					}
					else
					{
						var genArgs = mAttr.Dependencies.Select(dep => dep.BaseType?.GetGenericArguments()[0]).ToArray();
						inputTypes.AddRange(genArgs);
						genericTypes.AddRange(genArgs);
						args = mAttr.Dependencies.Select(dep => selectors[dep]).ToArray();
					}

					inputTypes.Add(method.Value.ReturnType);
					genericTypes.Add(method.Value.ReturnType);

					var delegateType = funcTypes[inputTypes.Count - 1].MakeGenericType(inputTypes.ToArray());
					var del = Delegate.CreateDelegate(delegateType, method.Value);

					var selectorType = selectorTypes
						.First(t => t.GenericTypeParameters.Length == genericTypes.Count)
						.MakeGenericType(genericTypes.ToArray());

					args = args.Concat(new[] { (object)del }).ToArray();
					var selector = Activator.CreateInstance(selectorType, args);
					selectors.Add(mAttr.MarkerType, selector);
				}
			}

			return selectors;
		}
	}
}
