using ReduxSharp.Store.Actions;

namespace ReduxSharp.TestSet1.Counter
{
	public partial class CounterState
	{
		public class SetCounter : IAction
		{
			public SetCounter(int payload)
			{
				Payload = payload;
			}

			public int Payload { get; }
		}

		public class SetCounterAsync : IAction
		{
			public SetCounterAsync(int payload)
			{
				Payload = payload;
			}

			public int Payload { get; }
		}

		public class Increment : IAction { }
		public class Decrement : IAction { }
	}
}
