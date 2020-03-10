using System;
using System.Collections.Generic;
using ReduxSharp.Store;
using ReduxSharp.Store.Selectors;
using ReduxSharp.Tests.Model;
using Xunit;

namespace ReduxSharp.Tests
{
	public class SelectorFactoryTest
	{
		[Fact]
		public void Test1()
		{
			var state = new AppStateModel(2, new SubStateModel(new[] {"name0", "name1", "name2", "name3"}));

			var store = new Store<AppStateModel>(state);

			int counter = 0;
			string name = null;

			store.Select(new GetCounter()).Subscribe(c => counter = c);
			store.Select(new GetSubStateName()).Subscribe(n => name = n);

			Assert.Equal(2, counter);
			Assert.Equal("name2", name);

			store.Dispatch(new SetCounter(0));

			Assert.Equal(0, counter);
			Assert.Equal("name0", name);
		}
	}
}
