using System;

namespace ReduxSharp.Store.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ActionAttribute : Attribute
	{
		public ActionAttribute(Type actionType)
		{
			ActionType = actionType;
		}

		public Type ActionType { get; }
	}
}
