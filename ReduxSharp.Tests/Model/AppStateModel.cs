namespace ReduxSharp.Tests.Model
{
	public class AppStateModel
	{
		public AppStateModel()
		{
			Counter = 0;
			SubState = new SubStateModel();
		}

		public AppStateModel(int counter, SubStateModel subState)
		{
			Counter = counter;
			SubState = subState;
		}

		public int Counter { get; }
		public SubStateModel SubState { get; }
	}
}
