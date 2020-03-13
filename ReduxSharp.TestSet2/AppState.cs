using ReduxSharp.Store.Actions;
using ReduxSharp.Store.Attributes;

namespace ReduxSharp.TestSet2
{
	[State(typeof(AppStateModel2))]
	public partial class AppState
	{
		[Action(typeof(SetCounter))]
		public void OnSetCounter(IStateContext<AppStateModel2> context, SetCounter action)
		{
			var state = context.GetState();
			state = new AppStateModel2(action.Payload, state.Names, state.KeyValues);
			context.SetState(state);
		}

		[Action(typeof(Increment))]
		public void OnIncrement(IStateContext<AppStateModel2> context)
		{
			var state = context.GetState();
			state = new AppStateModel2(state.Counter + 1, state.Names, state.KeyValues);
			context.SetState(state);
		}

		[Action(typeof(Decrement))]
		public void OnDecrement(IStateContext<AppStateModel2> context)
		{
			var state = context.GetState();
			state = new AppStateModel2(state.Counter - 1, state.Names, state.KeyValues);
			context.SetState(state);
		}

		[Action(typeof(SetNames))]
		public void OnSetCounter(IStateContext<AppStateModel2> context, SetNames action)
		{
			var state = context.GetState();
			state = new AppStateModel2(state.Counter, action.Payload, state.KeyValues);
			context.SetState(state);
		}

		[Action(typeof(SetConfig))]
		public void OnSetConfig(IStateContext<AppStateModel2> context, SetConfig action)
		{
			var state = context.GetState();
			state = new AppStateModel2(state.Counter, state.Names, action.Payload);
			context.SetState(state);
		}
	}
}
