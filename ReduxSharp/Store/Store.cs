using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using ReduxSharp.Store.Actions;
using ReduxSharp.Store.Selectors;

namespace ReduxSharp.Store
{
	public class Store<TState> where TState : class, new()
	{
		private readonly Dictionary<Type, object> _selectors;
		private readonly Dictionary<Type, Action<object>> _reducers;
		private readonly BehaviorSubject<TState> _observableState;

		public Store(TState initialState = null)
		{
			var assemebly = Assembly.GetCallingAssembly();
			_selectors = SelectorFactory.CreateSelectors(assemebly);
			_reducers = ActionFactory.CreateActions(assemebly, this);
			_observableState = new BehaviorSubject<TState>(initialState ?? new TState());
		}

		public IObservable<TState> Select()
		{
			return _observableState.DistinctUntilChanged();
		}

		public IObservable<TResult> Select<TResult>(ISelector<TState, TResult> selector)
		{
			return selector.Apply(_observableState);
		}

		public IObservable<TResult> Select<TResult>(SelectorMarker<TResult> marker)
		{
			return GetSelectorFromMarker(marker).Apply(_observableState);
		}

		public TResult Snapshot<TResult>(ISelector<TState, TResult> selector)
		{
			bool hasValue = _observableState.TryGetValue(out var value);
			return selector.Apply(hasValue ? value : null);
		}

		public TResult Snapshot<TResult>(SelectorMarker<TResult> marker)
		{
			bool hasValue = _observableState.TryGetValue(out var value);
			return GetSelectorFromMarker(marker).Apply(hasValue ? value : default(TState));
		}

		public void Dispatch<T>(IAction<T> action)
		{
			var reducer = _reducers[action.GetType()];
			reducer(action);
		}

		internal TState GetState()
		{
			return _observableState.Value;
		}

		internal void SetState(TState state)
		{
			_observableState.OnNext(state);
		}

		private ISelector<TState, TResult> GetSelectorFromMarker<TResult>(SelectorMarker<TResult> marker)
		{
			return (ISelector<TState, TResult>) _selectors[marker.GetType()];
		}
	}
}
