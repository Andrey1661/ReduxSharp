using System.Threading.Tasks;
using ReduxSharp.Store.Actions;
using ReduxSharp.Store.Attributes;

namespace ReduxSharp.TestSet1.Counter
{
	[State(typeof(CounterStateModel))]
	public partial class CounterState
	{
		[Action(typeof(SetCounter))]
		public void OnSetCounter(IStateContext<CounterStateModel> context, SetCounter action)
		{
			context.SetState(new CounterStateModel(action.Payload));
		}

		[Action(typeof(Increment))]
		public void OnIncrement(IStateContext<CounterStateModel> context)
		{
			context.SetState(new CounterStateModel(context.GetState().Value + 1));
		}

		[Action(typeof(Decrement))]
		public void OnDecrement(IStateContext<CounterStateModel> context)
		{
			context.SetState(new CounterStateModel(context.GetState().Value - 1));
		}

		[Effect(typeof(SetCounterRequested))]
		public async Task OnSetCounterEffect(IStateContext<CounterStateModel> context, SetCounterRequested action)
		{
			await Task.Delay(200);
			context.Dispatch(new SetCounterSuccess(action));
		}

		[Action(typeof(SetCounterSuccess))]
		public void OnSetCounterSuccess(IStateContext<CounterStateModel> context, SetCounterSuccess action)
		{
			context.SetState(new CounterStateModel(action.Payload.Payload));
		}
	}
}
