using System;

namespace ReduxSharp.Store.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class StateAttribute : Attribute
	{
		public StateAttribute(Type stateType)
		{
			StateType = stateType;
		}

		public Type StateType { get; }
	}
}
