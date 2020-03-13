using System;
using System.Collections.Generic;
using System.Linq;

namespace ReduxSharp
{
	internal class StoreContext
	{
		public StoreContext()
		{
			Selectors = new Dictionary<Type, object>();
			RootAssignFunctions = new Dictionary<Type, Delegate>();
		}

		public ILookup<Type, Action<object>> ActionHandlers { get; set; }
		public IDictionary<Type, object> Selectors { get; set; }
		public IDictionary<Type, Delegate> RootAssignFunctions { get; set; }
	}
}
