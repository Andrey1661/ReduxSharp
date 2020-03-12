using ReduxSharp.Store.Attributes;

namespace ReduxSharp.TestSet1.Counter
{
	[Query(typeof(CounterStateModel))]
	public static partial class CounterQuery
	{
		[Query(typeof(GetState))]
		public static CounterStateModel OnGetState(CounterStateModel state)
		{
			return state;
		}

		[Query(typeof(GetValue))]
		public static int OnGetValue([Query(typeof(GetState))] CounterStateModel state)
		{
			return state.Value;
		}
	}
}
