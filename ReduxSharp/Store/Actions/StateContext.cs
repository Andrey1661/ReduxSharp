namespace ReduxSharp.Store.Actions
{
	public class StateContext<TState> where TState : class, new()
	{
		private readonly Store<TState> _store;

		internal StateContext(Store<TState> store)
		{
			_store = store;
		}

		public TState GetState()
		{
			return _store.GetState();
		}

		public void SetState(TState state)
		{
			_store.SetState(state);
		}
	}
}
