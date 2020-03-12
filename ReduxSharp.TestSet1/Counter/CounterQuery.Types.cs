using ReduxSharp.Store.Selectors;

namespace ReduxSharp.TestSet1.Counter
{
	public static partial class CounterQuery
	{
		public class GetState : IQuery<CounterStateModel> { }
		public class GetValue : IQuery<int> { }
	}
}
