using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReduxSharp.Store;
using ReduxSharp.TestSet1;
using ReduxSharp.TestSet1.Array;
using ReduxSharp.TestSet1.Config;
using ReduxSharp.TestSet1.Counter;

namespace ReduxSharp.Tests
{
	class Program
	{
		public static void Main(string[] args)
		{
			var store = CreateStore();
			var task = store.Select(new CounterQuery.GetValue()).Skip(1).Take(1).ToTask();
			store.Dispatch(new CounterState.SetCounterRequested(10));
			task.Wait();
			Console.WriteLine(task.Result);
			Console.ReadKey();
		}

		private static Store<AppStateModel1> CreateStore()
		{
			var state = new AppStateModel1(
				new CounterStateModel(2),
				new ArrayStateModel(new[] { "name0", "name1", "name2", "name3" }),
				new ConfigStateModel(new Dictionary<string, object>
				{
					{"key1", "value1"},
					{"key2", "value2"}
				})
			);
			return new Store<AppStateModel1>(state);
		}
	}
}
