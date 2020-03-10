using System;

namespace ReduxSharp.Store.Selectors
{
	public interface ISelector<in TState, out TResult>
	{
		TResult Apply(TState arg);
		IObservable<TResult> Apply(IObservable<TState> arg);
	}
}
