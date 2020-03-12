using System;
using System.Collections.Generic;
using System.Linq;

namespace ReduxSharp
{
	internal static class ExchangeStorage
	{
		static ExchangeStorage()
		{
			Selectors = new Dictionary<Type, object>();
			RootAssignFunctions = new Dictionary<Type, Delegate>();
		}

		public static ILookup<Type, Action<object>> ActionHandlers { get; set; }
		public static IDictionary<Type, object> Selectors { get; set; }
		public static IDictionary<Type, Delegate> RootAssignFunctions { get; set; }
	}
}
