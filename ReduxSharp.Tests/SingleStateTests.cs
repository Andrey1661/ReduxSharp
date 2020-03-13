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
		public void Test1()
		{
			var store = CreateStore();

			int counter = 0;
			string name = null;
			string[] names = null;

			store.Select(new AppStateQuery.GetCounter()).Subscribe(c => counter = c);
			store.Select(new AppStateQuery.GetName()).Subscribe(n => name = n);
			store.Select(new AppStateQuery.GetNames()).Subscribe(n => names = n);

			Assert.Equal(2, counter);
			Assert.Equal("name2", name);

			store.Dispatch(new AppState.SetCounter(0));

			Assert.Equal(0, counter);
			Assert.Equal("name0", name);

			var reversed = names.Reverse().ToArray();
			store.Dispatch(new AppState.SetNames(reversed));

			Assert.Equal(reversed, names);
			Assert.Equal("name3", name);
		}


	}
}
