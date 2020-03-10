using System;

namespace ReduxSharp.Store.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class StateAttribute : Attribute
	{
		public Type StateType { get; }

		public StateAttribute(Type stateType)
		{
			StateType = stateType;
		}
	}
}
