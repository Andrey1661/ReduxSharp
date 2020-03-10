using System;
using System.Reactive.Linq;

namespace ReduxSharp.Store.Selectors
{
	internal class Selector<TState, TResult> : ISelector<TState, TResult>
	{
		private readonly Func<TState, TResult> _selectorFunc;

		public Selector(Func<TState, TResult> selectorFunc)
		{
			_selectorFunc = selectorFunc;
		}

		public TResult Apply(TState arg)
		{
			return _selectorFunc(arg);
		}

		public IObservable<TResult> Apply(IObservable<TState> arg)
		{
			return arg.Select(_selectorFunc).DistinctUntilChanged();
		}
	}

	internal class Selector<TState, TRes1, TResult> : ISelector<TState, TResult>
	{
		private readonly Func<TRes1, TResult> _selectorFunc;
		private readonly ISelector<TState, TRes1> _selector;

		public Selector(ISelector<TState, TRes1> selector, Func<TRes1, TResult> selectorFunc)
		{
			_selector = selector;
			_selectorFunc = selectorFunc;
		}

		public TResult Apply(TState arg)
		{
			var result = _selector.Apply(arg);
			return _selectorFunc(result);
		}

		public IObservable<TResult> Apply(IObservable<TState> arg)
		{
			return _selector.Apply(arg)
				.Select(_selectorFunc)
				.DistinctUntilChanged();
		}
	}

	internal class Selector<TState, TRes1, TRes2, TResult> : ISelector<TState, TResult>
	{
		private readonly Func<TRes1, TRes2, TResult> _selectorFunc;
		private readonly ISelector<TState, TRes1> _selector1;
		private readonly ISelector<TState, TRes2> _selector2;

		public Selector(
			ISelector<TState, TRes1> selector1,
			ISelector<TState, TRes2> selector2,
			Func<TRes1, TRes2, TResult> selectorFunc)
		{
			_selector1 = selector1;
			_selector2 = selector2;
			_selectorFunc = selectorFunc;
		}

		public TResult Apply(TState arg)
		{
			var res1 = _selector1.Apply(arg);
			var res2 = _selector2.Apply(arg);
			return _selectorFunc(res1, res2);
		}

		public IObservable<TResult> Apply(IObservable<TState> arg)
		{
			return _selector1.Apply(arg).CombineLatest(
					_selector2.Apply(arg),
					Tuple.Create)
				.Select(res => _selectorFunc(
					res.Item1,
					res.Item2))
				.DistinctUntilChanged();
		}
	}

	internal class Selector<TState, TRes1, TRes2, TRes3, TResult> : ISelector<TState, TResult>
	{
		private readonly Func<TRes1, TRes2, TRes3, TResult> _selectorFunc;
		private readonly ISelector<TState, TRes1> _selector1;
		private readonly ISelector<TState, TRes2> _selector2;
		private readonly ISelector<TState, TRes3> _selector3;

		public Selector(
			ISelector<TState, TRes1> selector1,
			ISelector<TState, TRes2> selector2,
			ISelector<TState, TRes3> selector3,
			Func<TRes1, TRes2, TRes3, TResult> selectorFunc)
		{
			_selector1 = selector1;
			_selector2 = selector2;
			_selector3 = selector3;
			_selectorFunc = selectorFunc;
		}

		public TResult Apply(TState arg)
		{
			var res1 = _selector1.Apply(arg);
			var res2 = _selector2.Apply(arg);
			var res3 = _selector3.Apply(arg);
			return _selectorFunc(res1, res2, res3);
		}

		public IObservable<TResult> Apply(IObservable<TState> arg)
		{
			return _selector1.Apply(arg).CombineLatest(
					_selector2.Apply(arg),
					_selector3.Apply(arg), 
					Tuple.Create)
				.Select(res => _selectorFunc(
					res.Item1,
					res.Item2,
					res.Item3))
				.DistinctUntilChanged();
		}
	}

	internal class Selector<TState, TRes1, TRes2, TRes3, TRes4, TResult> : ISelector<TState, TResult>
	{
		private readonly Func<TRes1, TRes2, TRes3, TRes4, TResult> _selectorFunc;
		private readonly ISelector<TState, TRes1> _selector1;
		private readonly ISelector<TState, TRes2> _selector2;
		private readonly ISelector<TState, TRes3> _selector3;
		private readonly ISelector<TState, TRes4> _selector4;

		public Selector(
			ISelector<TState, TRes1> selector1,
			ISelector<TState, TRes2> selector2,
			ISelector<TState, TRes3> selector3,
			ISelector<TState, TRes4> selector4,
			Func<TRes1, TRes2, TRes3, TRes4, TResult> selectorFunc)
		{
			_selector1 = selector1;
			_selector2 = selector2;
			_selector3 = selector3;
			_selector4 = selector4;
			_selectorFunc = selectorFunc;
		}

		public TResult Apply(TState arg)
		{
			var res1 = _selector1.Apply(arg);
			var res2 = _selector2.Apply(arg);
			var res3 = _selector3.Apply(arg);
			var res4 = _selector4.Apply(arg);
			return _selectorFunc(res1, res2, res3, res4);
		}

		public IObservable<TResult> Apply(IObservable<TState> arg)
		{
			return _selector1.Apply(arg).CombineLatest(
					_selector2.Apply(arg),
					_selector3.Apply(arg),
					_selector4.Apply(arg),
					Tuple.Create)
				.Select(res => _selectorFunc(
					res.Item1,
					res.Item2,
					res.Item3,
					res.Item4))
				.DistinctUntilChanged();
		}
	}

	internal class Selector<TState, TRes1, TRes2, TRes3, TRes4, TRes5, TResult> : ISelector<TState, TResult>
	{
		private readonly Func<TRes1, TRes2, TRes3, TRes4, TRes5, TResult> _selectorFunc;
		private readonly ISelector<TState, TRes1> _selector1;
		private readonly ISelector<TState, TRes2> _selector2;
		private readonly ISelector<TState, TRes3> _selector3;
		private readonly ISelector<TState, TRes4> _selector4;
		private readonly ISelector<TState, TRes5> _selector5;

		public Selector(
			ISelector<TState, TRes1> selector1,
			ISelector<TState, TRes2> selector2,
			ISelector<TState, TRes3> selector3,
			ISelector<TState, TRes4> selector4,
			ISelector<TState, TRes5> selector5,
			Func<TRes1, TRes2, TRes3, TRes4, TRes5, TResult> selectorFunc)
		{
			_selector1 = selector1;
			_selector2 = selector2;
			_selector3 = selector3;
			_selector4 = selector4;
			_selector5 = selector5;
			_selectorFunc = selectorFunc;
		}

		public TResult Apply(TState arg)
		{
			var res1 = _selector1.Apply(arg);
			var res2 = _selector2.Apply(arg);
			var res3 = _selector3.Apply(arg);
			var res4 = _selector4.Apply(arg);
			var res5 = _selector5.Apply(arg);
			return _selectorFunc(res1, res2, res3, res4, res5);
		}

		public IObservable<TResult> Apply(IObservable<TState> arg)
		{
			return _selector1.Apply(arg).CombineLatest(
					_selector2.Apply(arg),
					_selector3.Apply(arg),
					_selector4.Apply(arg),
					_selector5.Apply(arg),
					Tuple.Create)
				.Select(res => _selectorFunc(
					res.Item1,
					res.Item2,
					res.Item3,
					res.Item4,
					res.Item5))
				.DistinctUntilChanged();
		}
	}
}
