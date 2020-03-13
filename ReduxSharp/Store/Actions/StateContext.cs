﻿using System;
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

		public StateContext(Store<TRoot> store)
		{
			_store = store;
		}

		public TRoot GetState()
		{
			return _store.Snapshot();
		}

		public void SetState(TRoot state)
		{
			_store.SetState(state);
		}
	}

	internal class StateContext<TRoot, TState> : IStateContext<TState>
		where TRoot : class, ICloneable, new()
		where TState : class, ICloneable, new()
	{
		private readonly Store<TRoot> _store;
		private readonly ISelector<TRoot, TState> _selector;
		private readonly Func<TRoot, TState, TRoot> _assignDelegate;

		public StateContext(
			Store<TRoot> store,
			ISelector<TRoot, TState> selector,
			Func<TRoot, TState, TRoot> assignDelegate)
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
			var root = _assignDelegate(_store.GetState(), state);
			_store.SetState(root);
		}
	}
}
