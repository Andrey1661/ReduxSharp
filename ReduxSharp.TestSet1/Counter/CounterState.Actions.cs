using ReduxSharp.Store.Actions;

namespace ReduxSharp.TestSet1.Counter
{
	public partial class CounterState
	{
		public class SetCounter : IAction<int>
		{
			public SetCounter(int payload)
			{
				Payload = payload;
			}

			public int Payload { get; }
		}

		public class SetCounterRequested : IAction<int>
		{
			public SetCounterRequested(int payload)
			{
				Payload = payload;
			}

			public int Payload { get; }
		}

		public class SetCounterSuccess : IAction<SetCounterRequested>
		{
			public SetCounterSuccess(SetCounterRequested payload)
			{
				Payload = payload;
			}

			public SetCounterRequested Payload { get; }
		}

		public class Increment : IAction { }
		public class Decrement : IAction { }
	}
}
