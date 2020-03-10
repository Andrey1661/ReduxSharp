using ReduxSharp.Store.Actions;
using ReduxSharp.Store.Attributes;
using ReduxSharp.Tests.Model;

namespace ReduxSharp.Tests
{
	[State(typeof(AppStateModel))]
	public class AppState
	{
		[Action(typeof(SetCounter))]
		public void SetCounter(StateContext<AppStateModel> context, SetCounter action)
		{
			var state = context.GetState();
			state = new AppStateModel(action.Payload, state.SubState);
			context.SetState(state);
		}
	}
}
