using ReduxSharp.Store.Actions;

namespace ReduxSharp.Tests
{
	public class SetCounter : IAction<int>
	{
		public SetCounter(int value)
		{
			Payload = value;
		}

		public int Payload { get; }
	}
}
