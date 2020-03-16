using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReduxSharp.Store.Actions;
using ReduxSharp.Store.Selectors;

namespace ReduxSharp.Store
{
	public class Store<TState> where TState : class, ICloneable<TState>, new()
	{
		private readonly BehaviorSubject<TState> _observableState;
		private TState _tempState;

		public Store(TState initialState = null)
		{
			Context = new StoreContext();
			SelectorFactory.CreateSelectors(this);
			ActionFactory.CreateActions(this);
			_observableState = new BehaviorSubject<TState>(initialState ?? new TState());
		}

		#region public members

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

		public TState Snapshot()
		{
			return _observableState.Value;
		}

		public TResult Snapshot<TResult>(IQuery<TResult> selector)
		{
			return GetSelector(selector).Apply(_observableState.Value);
		}

		public void Dispatch<T>(IAction<T> action)
		{
			var reducers = Context.ActionHandlers[action.GetType()];
			using (new DispatchSequence<TState>(this))
			{
				foreach (var reducer in reducers)
				{
					reducer(action);
				}
			}
		}

		#endregion

		#region internal members

		internal StoreContext Context { get; }

		internal bool SequenceDispatchMode
		{
			set
			{
				if (value)
				{
					_tempState = _observableState.Value;
				}
				else
				{
					_observableState.OnNext(_tempState);
				}
			}
		}

		internal TResult Snapshot<TResult>(ISelector<TState, TResult> selector)
		{
			return selector.Apply(_observableState.Value);
		}

		internal TState GetState()
		{
			return _tempState.Clone();
		}

		internal void SetState(TState state)
		{
			_tempState = state;
		}

		internal void DispatchInternal<T>(IAction<T> action)
		{
			var reducers = Context.ActionHandlers[action.GetType()];
			foreach (var reducer in reducers)
			{
				reducer(action);
			}
		}

		#endregion

		#region private members

		private ISelector<TState, TResult> GetSelector<TResult>(IQuery<TResult> marker)
		{
			return (ISelector<TState, TResult>) Context.Selectors[marker.GetType()];
		}

		#endregion
	}
}
