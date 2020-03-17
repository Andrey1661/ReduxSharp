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

		[Effect(typeof(SetCounterAsync), EffectStage.Request)]
		public bool OnSetCounterRequest(IStateContext<CounterStateModel> context, SetCounterAsync action)
		{
			return action.Payload != 10;
		}

		[Effect(typeof(SetCounterAsync), EffectStage.Effect)]
		public async Task<int> OnSetCounterEffect(IStateContext<CounterStateModel> context, SetCounterAsync action)
		{
			await Task.Delay(100);
			return action.Payload;
		}

		[Effect(typeof(SetCounterAsync), EffectStage.Success)]
		public void OnSetCounterSuccess(IStateContext<CounterStateModel> context, int result)
		{
			context.SetState(new CounterStateModel(result));
		}
	}
}
