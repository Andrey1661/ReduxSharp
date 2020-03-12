using System;
using System.Collections.Generic;
using ReduxSharp.Store;
using ReduxSharp.TestSet1;
using ReduxSharp.TestSet1.Array;
using ReduxSharp.TestSet1.Config;
using ReduxSharp.TestSet1.Counter;
using Xunit;

namespace ReduxSharp.Tests
{
	public class SelectorFactoryTest
	{
		private Store<AppStateModel> CreateStore()
		{
			var state = new AppStateModel(
				new CounterStateModel(2),
				new ArrayStateModel(new[] {"name0", "name1", "name2", "name3"}),
				new ConfigStateModel(new Dictionary<string, object>
				{
					{"key1", "value1"},
					{"key2", "value2"}
				})
			);
			return new Store<AppStateModel>(typeof(AppStateModel).Assembly, state);
		}

		[Fact]
		public void Test1()
		{
			var store = CreateStore();

			int counter = 0;
			string name = null;

			store.Select(new CounterQuery.GetValue()).Subscribe(c => counter = c);
			store.Select(new ArrayQuery.GetName()).Subscribe(n => name = n);

			Assert.Equal(2, counter);
			Assert.Equal("name2", name);

			store.Dispatch(new CounterState.SetCounter(0));

			Assert.Equal(0, counter);
			Assert.Equal("name0", name);
		}
	}
}