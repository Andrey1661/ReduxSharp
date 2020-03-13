using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ReduxSharp.Store.Attributes;
using ReduxSharp.Store.Selectors;

namespace ReduxSharp.Store.Actions
{
	internal static class ActionFactory
	{
		internal static void CreateActions<TRoot>(Assembly assembly, Store<TRoot> store) where TRoot : class, ICloneable, new()
		{
			var stateTypes = assembly.DefinedTypes.Where(type => type.GetCustomAttribute<StateAttribute>() != null);
			var result = new List<(Type type, Action<object> actions)>();

			foreach (var stateType in stateTypes)
			{
				var stateInstance = Activator.CreateInstance(stateType);
				var stateAttr = stateType.GetCustomAttribute<StateAttribute>();
				var contextInstance = CreateContext(store, stateAttr.StateType);
				var contextType = contextInstance.GetType().GetInterfaces().First();

				foreach (var method in stateType.DeclaredMethods.Where(m => m.IsPublic && m.GetCustomAttribute<ActionAttribute>() != null))
				{
					var attr = method.GetCustomAttribute<ActionAttribute>();
					var isGeneric = attr.ActionType.GetInterfaces().First().IsGenericType;
					var delegateType = isGeneric
						? typeof(Action<,>).MakeGenericType(contextType, attr.ActionType)
						: typeof(Action<>).MakeGenericType(contextType);
					var del = Delegate.CreateDelegate(delegateType, stateInstance, method);

					result.Add((attr.ActionType, Handler));

					void Handler(object action)
					{
						del.DynamicInvoke(contextInstance, action);
					}
				}
			}

			ExchangeStorage.ActionHandlers = result.ToLookup(pair => pair.type, pair => pair.actions);
		}

		private static object CreateContext<TRoot>(Store<TRoot> store, Type stateType)
			where TRoot : class, ICloneable, new()
		{
			var rootType = typeof(TRoot);
			Type contextType;
			object[] parameters;

			if (rootType == stateType)
			{
				contextType = typeof(StateContext<>).MakeGenericType(stateType);
				parameters = new object[] {store};
			}
			else
			{
				contextType = typeof(StateContext<,>).MakeGenericType(rootType, stateType);
				if (!ExchangeStorage.Selectors.ContainsKey(stateType))
				{
					ExchangeStorage.Selectors.Add(stateType, SelectorFactory.CreateRootSelector(rootType, stateType));
				}

				var rootSelector = ExchangeStorage.Selectors[stateType];
				var assignDelegate = GetOrCreateAssignFunction(rootType, stateType);
				parameters = new[] {store, rootSelector, assignDelegate};
			}

			return Activator.CreateInstance(
				contextType,
				BindingFlags.Public | BindingFlags.Instance,
				null,
				parameters,
				null
			);
		}

		private static Delegate GetOrCreateAssignFunction(Type rootType, Type stateType)
		{
			if (ExchangeStorage.RootAssignFunctions.TryGetValue(stateType, out var del))
			{
				return del;
			}

			var assignDelegate = CreateAssignFunction(rootType, stateType);
			ExchangeStorage.RootAssignFunctions.Add(stateType, assignDelegate);
			return assignDelegate;
		}

		private static Delegate CreateAssignFunction(Type rootType, Type stateType)
		{
			var constr = rootType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
				.OrderByDescending(c => c.GetParameters().Length)
				.First();
			var properties = rootType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var rootParamExpr = Expression.Parameter(rootType, "root");
			var stateParamExpr = Expression.Parameter(stateType, "state");
			var paramExpressions = constr.GetParameters()
				.Select(p => p.ParameterType == stateType
					? (Expression) stateParamExpr
					: Expression.Property(rootParamExpr, properties.First(prop => prop.PropertyType == p.ParameterType)))
				.ToArray();
			var newExpr = Expression.New(constr, paramExpressions);
			var lambdaExpr = Expression.Lambda(newExpr, rootParamExpr, stateParamExpr);
			return lambdaExpr.Compile();
		}
	}
}
