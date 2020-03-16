using System;

namespace ReduxSharp.Store.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class EffectAttribute : ActionAttribute
	{
		public EffectAttribute(Type actionType) : base(actionType)
		{
		}
	}
}
