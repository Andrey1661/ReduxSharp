using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ReduxSharp.Store.Attributes;
using ReduxSharp.Store.Selectors;

namespace ReduxSharp.Store.Actions
{
	internal static class ActionFactory
	{
		internal static void CreateActions<TRoot>(Store<TRoot> store) where TRoot : class, ICloneable<TRoot>, new()
		{
			var rootType = typeof(TRoot);
			var stateTypes = rootType.Assembly.DefinedTypes.Where(type => Attribute.IsDefined(type, typeof(StateAttribute)));
			var result = new List<(Type type, Action<object> actions)>();

			foreach (var stateType in stateTypes)
			{
				var stateInstance = Activator.CreateInstance(stateType);
				var stateAttr = stateType.GetCustomAttribute<StateAttribute>();
				var contextInstance = CreateContext(store, stateAttr.StateType);
				var contextType = contextInstance.GetType().GetInterfaces().First();

				foreach (var method in stateType.DeclaredMethods.Where(m =>
					m.IsPublic && Attribute.IsDefined(m, typeof(ActionAttribute))))
				{
					var attr = method.GetCustomAttribute<ActionAttribute>();

					if (attr is EffectAttribute effectAttr)
					{
						var effectHandler = CreateEffectHandler(method, stateInstance, contextInstance, effectAttr);
						result.Add((attr.ActionType, effectHandler));
						continue;
					}

					var hasActionArg = method.GetParameters().Length == 2;
					var delegateType = hasActionArg
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

			store.Context.ActionHandlers = result.ToLookup(pair => pair.type, pair => pair.actions);
		}

		private static Action<object> CreateEffectHandler(MethodInfo method, object stateInstance, object contextInstance, EffectAttribute attr)
		{
			bool isAsync = Attribute.IsDefined(method, typeof(AsyncStateMachineAttribute));
			var delType = isAsync
				? typeof(Func<,,>).MakeGenericType(contextInstance.GetType(), attr.ActionType, typeof(Task))
				: typeof(Action<,>).MakeGenericType(contextInstance.GetType(), attr.ActionType);
			var del = Delegate.CreateDelegate(delType, stateInstance, method);

			return isAsync ? (Action<object>)Handler : AsyncHandler;

			void Handler(object action)
			{
				del.DynamicInvoke(contextInstance, action);
			}

			async void AsyncHandler(object action)
			{
				try
				{
					await ((Task)del.DynamicInvoke(contextInstance, action)).ConfigureAwait(false);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}
		}

		private static object CreateContext<TRoot>(Store<TRoot> store, Type stateType)
			where TRoot : class, ICloneable<TRoot>, new()
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
				if (!store.Context.Selectors.ContainsKey(stateType))
				{
					store.Context.Selectors.Add(stateType, SelectorFactory.CreateRootSelector(rootType, stateType));
				}

				var rootSelector = store.Context.Selectors[stateType];
				var assignDelegate = GetOrCreateAssignFunction(store.Context, rootType, stateType);
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

		private static Delegate GetOrCreateAssignFunction(StoreContext context, Type rootType, Type stateType)
		{
			if (context.RootAssignFunctions.TryGetValue(stateType, out var del))
			{
				return del;
			}

			var assignDelegate = CreateAssignFunction(rootType, stateType);
			context.RootAssignFunctions.Add(stateType, assignDelegate);
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