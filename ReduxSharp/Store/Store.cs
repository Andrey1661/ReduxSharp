using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReduxSharp.Store.Actions;
using ReduxSharp.Store.Selectors;

namespace ReduxSharp.Store
{
	public class Store<TState> where TState : class, ICloneable, new()
	{
		private readonly BehaviorSubject<TState> _observableState;

		public Store(TState initialState = null)
		{
			var assembly = typeof(TState).Assembly;
			SelectorFactory.CreateSelectors(assembly, typeof(TState));
			ActionFactory.CreateActions(assembly, this);
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

		public IObservable<TResult> Select<TResult>(IQuery<TResult> selector)
		{
			return GetSelector(selector).Apply(_observableState);
		}

		internal TResult Snapshot<TResult>(ISelector<TState, TResult> selector)
		{
			bool hasValue = _observableState.TryGetValue(out var value);
			return selector.Apply(hasValue ? value : null);
		}

		public TState Snapshot()
		{
			return _observableState.Value;
		}

		public TResult Snapshot<TResult>(IQuery<TResult> selector)
		{
			bool hasValue = _observableState.TryGetValue(out var value);
			return GetSelector(selector).Apply(hasValue ? value : default(TState));
		}

		public void Dispatch<T>(IAction<T> action)
		{
			var reducers = ExchangeStorage.ActionHandlers[action.GetType()];
			foreach (var reducer in reducers)
			{
				reducer(action);
			}
		}

		internal TState GetState()
		{
			return _observableState.Value;
		}

		internal void SetState(TState state)
		{
			_observableState.OnNext(state);
		}

		private static ISelector<TState, TResult> GetSelector<TResult>(IQuery<TResult> marker)
		{
			return (ISelector<TState, TResult>) ExchangeStorage.Selectors[marker.GetType()];
		}
	}
}
