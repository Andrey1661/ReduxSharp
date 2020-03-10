using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReduxSharp.Store.Attributes;

namespace ReduxSharp.Store.Actions
{
	internal static class ActionFactory
	{
		internal static Dictionary<Type, Action<object>> CreateActions<TState>(Assembly assembly, Store<TState> store) where TState: class, new()
		{
			var stateTypes = assembly.DefinedTypes.Where(type => type.GetCustomAttribute<StateAttribute>() != null);
			var handlers = new Dictionary<Type, Action<object>>();

			foreach (var stateType in stateTypes)
			{
				var stateAttr = stateType.GetCustomAttribute<StateAttribute>();
				var contextType = typeof(StateContext<>).MakeGenericType(stateAttr.StateType);
				var instance = Activator.CreateInstance(stateType);
				var contextInstance = Activator.CreateInstance(
					type: contextType,
					bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance,
					binder: null,
					args: new object[] {store},
					culture: null
				);

				foreach (var method in stateType.DeclaredMethods.Where(m => m.IsPublic && m.GetCustomAttribute<ActionAttribute>() != null))
				{
					var attr = method.GetCustomAttribute<ActionAttribute>();
					var delegateType = typeof(Action<,>).MakeGenericType(contextType, attr.ActionType);
					var del = Delegate.CreateDelegate(delegateType, instance, method);			

					handlers.Add(attr.ActionType, Handler);

					void Handler(object action)
					{
						del.DynamicInvoke(contextInstance, action);
					}
				}
			}

			return handlers;
		}
	}
}
