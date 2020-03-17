using System;

namespace ReduxSharp.Store.Attributes
{
	public enum EffectStage
	{
		Request,
		Effect,
		Success,
		Fail
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class EffectAttribute : ActionAttribute
	{
		public EffectStage Stage { get; }

		public EffectAttribute(Type actionType, EffectStage stage) : base(actionType)
		{
			Stage = stage;
		}
	}
}
