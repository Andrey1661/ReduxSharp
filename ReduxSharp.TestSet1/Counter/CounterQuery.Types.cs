using ReduxSharp.Store.Selectors;

namespace ReduxSharp.TestSet1.Counter
{
	public static partial class CounterQuery
	{
		public class GetState : Query<GetState, CounterStateModel> { }
		public class GetValue : Query<GetValue, int> { }
	}
}
