using System;

namespace ReduxSharp.Store
{
	public class DispatchSequence<TState> : IDisposable
		where TState : class, ICloneable<TState>, new()
	{
		private readonly Store<TState> _store;

		public DispatchSequence(Store<TState> store)
		{
			_store = store;
			_store.SequenceDispatchMode = true;
		}

		public void Dispose()
		{
			_store.SequenceDispatchMode = false;
		}
	}
}
