using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ReduxSharp.Store.Attributes;

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
				var contextInstance = CreateContext(store, stateType);
				var contextType = contextInstance.GetType();

				foreach (var method in stateType.DeclaredMethods.Where(m => m.IsPublic && m.GetCustomAttribute<ActionAttribute>() != null))
				{
					var attr = method.GetCustomAttribute<ActionAttribute>();
					var delegateType = typeof(Action<,>).MakeGenericType(contextType, attr.ActionType);
					var del = Delegate.CreateDelegate(delegateType, contextInstance, method);

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
				var rootSelector = ExchangeStorage.Selectors[stateType];
				var assignDelegate = GetOrCreateAssignFunction(rootType, stateType);
				parameters = new[] {store, rootSelector, assignDelegate};
			}

			return Activator.CreateInstance(
				contextType,
				BindingFlags.NonPublic | BindingFlags.Instance,
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
			var constr = rootType.GetConstructors(BindingFlags.Public)
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
