using System;
using System.Collections.Generic;
using System.Linq;
using ReduxSharp.Store;
using ReduxSharp.TestSet2;
using Xunit;

namespace ReduxSharp.Tests
{
	public class SingleStateTests
	{
		private Store<AppStateModel2> _store;
		private Store<AppStateModel2> Store => _store ?? (_store = CreateStore());

		private Store<AppStateModel2> CreateStore()
		{
			var state = new AppStateModel2(
				2,
				new[] { "name0", "name1", "name2", "name3" },
				new Dictionary<string, object>
				{
					{"key1", "value1"},
					{"key2", "value2"}
				}
			);
			return new Store<AppStateModel2>(state);
		}

		[Fact]
		public void BaseTest()
		{
			int counter = 0;
			string name = null;
			string[] names = null;

			Store.Select(new AppStateQuery.GetCounter()).Subscribe(c => counter = c);
			Store.Select(new AppStateQuery.GetName()).Subscribe(n => name = n);
			Store.Select(new AppStateQuery.GetNames()).Subscribe(n => names = n);

			Assert.Equal(2, counter);
			Assert.Equal("name2", name);

			Store.Dispatch(new AppState.SetCounter(0));

			Assert.Equal(0, counter);
			Assert.Equal("name0", name);

			var reversed = names.Reverse().ToArray();
			Store.Dispatch(new AppState.SetNames(reversed));

			Assert.Equal(reversed, names);
			Assert.Equal("name3", name);
		}

		[Fact]
		public void BigSelectorTest()
		{
			int sum = 0;

			Store.Select(AppStateQuery.GetCounterSum.Default).Subscribe(s => sum = s);
			Assert.Equal(14, sum);

			Store.Dispatch(new AppState.SetCounter(3));
			Assert.Equal(21, sum);
		}
	}
}
