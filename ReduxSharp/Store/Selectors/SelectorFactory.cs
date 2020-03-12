using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ReduxSharp.Store.Attributes;

namespace ReduxSharp.Store.Selectors
{
	internal static class SelectorFactory
	{
		internal static void CreateSelectors(Assembly assembly, Type rootType)
		{
			var selectors = new Dictionary<Type, object>();
			var curAssembly = Assembly.GetExecutingAssembly();
			var funcTypes = Assembly.GetAssembly(typeof(Func<>))
				.ExportedTypes.Where(t => t.Name.StartsWith("Func"))
				.OrderBy(t => t.GenericTypeArguments.Length).ToList();
			var queryTypes = assembly.DefinedTypes.Where(type => type.GetCustomAttribute<QueryAttribute>() != null);
			var selectorTypes = curAssembly.DefinedTypes.Where(t => t.Name.StartsWith("Selector") && t.IsNotPublic).ToList();

			foreach (var type in queryTypes)
			{
				var attr = type.GetCustomAttribute<QueryAttribute>();
				var stateType = attr.MarkerType;
				var methods = type.DeclaredMethods.Where(m => m.IsPublic && m.IsStatic && m.GetCustomAttribute<QueryAttribute>() != null).ToArray();

				foreach (var method in methods)
				{
					var methodAttr = method.GetCustomAttribute<QueryAttribute>();
					var parameters = method.GetParameters();

					List<Type> genericTypes = new List<Type>();
					List<Type> inputTypes = new List<Type>();
					List<object> args = new List<object>();

					switch (parameters.Length)
					{
						case 0:
							continue;
						case 1 when parameters[0].ParameterType == stateType && parameters[0].GetCustomAttribute<QueryAttribute>() == null:
							if (!selectors.ContainsKey(stateType))
							{
								selectors[stateType] = CreateRootSelector(rootType, stateType);
							}
							genericTypes.Add(stateType);
							inputTypes.Add(stateType);
							args.Add(selectors[stateType]);
							break;
						default:
							foreach (var parameter in parameters)
							{
								var paramQueryType = parameter.GetCustomAttribute<QueryAttribute>()?.MarkerType ?? parameter.ParameterType;
								inputTypes.Add(paramQueryType);
								genericTypes.Add(parameter.ParameterType);
								args.Add(selectors[paramQueryType]);
							}
							break;
					}

					if (genericTypes.FirstOrDefault() != stateType)
					{
						genericTypes.Insert(0, stateType);
					}

					inputTypes.Add(method.ReturnType);
					genericTypes.Add(method.ReturnType);

					var delegateType = funcTypes[inputTypes.Count - 1]
						.MakeGenericType(inputTypes.ToArray());
					var del = Delegate.CreateDelegate(delegateType, method);
					var selectorType = selectorTypes
						.First(t => t.GenericTypeParameters.Length == genericTypes.Count)
						.MakeGenericType(genericTypes.ToArray());

					args.Add(del);
					var selector = Activator.CreateInstance(
						selectorType,
						BindingFlags.Public | BindingFlags.Instance,
						null,
						args.ToArray(),
						null
					);
					var markerType = method.GetCustomAttribute<QueryAttribute>().MarkerType;
					selectors.Add(markerType, selector);
				}
			}

			ExchangeStorage.Selectors = selectors;
		}

		private static object CreateRootSelector(Type rootType, Type stateType)
		{
			if (rootType == stateType)
			{
				return Activator.CreateInstance(typeof(Selector<>).MakeGenericType(rootType));
			}

			var paramExpr = Expression.Parameter(rootType, "root");
			var propExpr = Expression.Property(paramExpr,
				rootType.GetRuntimeProperties().First(p => p.CanRead && p.PropertyType == stateType));
			var lambdaExpr = Expression.Lambda(propExpr, paramExpr);
			var del = lambdaExpr.Compile();

			var selectorType = typeof(Selector<,>).MakeGenericType(rootType, stateType);
			return Activator.CreateInstance(selectorType, del);
		}
	}
}
