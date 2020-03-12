using System;
using ReduxSharp.Store.Selectors;

namespace ReduxSharp.Store.Actions
{
	public interface IStateContext<TState>
		where TState : class, ICloneable, new()
	{
		TState GetState();
		void SetState(TState state);
	}

	internal class StateContext<TRoot> : IStateContext<TRoot>
		where TRoot : class, ICloneable, new ()
	{
		private readonly Store<TRoot> _store;

		public TRoot GetState()
		{
			return _store.Snapshot();
		}

		public void SetState(TRoot state)
		{
			throw new NotImplementedException();
		}
	}

	internal class StateContext<TRoot, TState> : IStateContext<TState>
		where TRoot : class, ICloneable, new()
		where TState : class, ICloneable, new()
	{
		private readonly Store<TRoot> _store;
		private readonly ISelector<TRoot, TState> _selector;
		private readonly Func<TState, TRoot> _assignDelegate;

		internal StateContext(
			Store<TRoot> store,
			ISelector<TRoot, TState> selector,
			Func<TState, TRoot> assignDelegate)
		{
			_store = store;
			_selector = selector;
			_assignDelegate = assignDelegate;
		}

		public TState GetState()
		{
			return _store.Snapshot(_selector);
		}

		public void SetState(TState state)
		{
			var root = _assignDelegate(state);
			_store.SetState(root);
		}
	}
}
