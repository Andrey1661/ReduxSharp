using System.Collections.Generic;
using System.Linq;
using ReduxSharp.Store.Attributes;

namespace ReduxSharp.TestSet2
{
	[Query(typeof(AppStateModel2))]
	public static partial class AppStateQuery
	{
		[Query(typeof(GetState))]
		public static AppStateModel2 OnGetState(AppStateModel2 state)
		{
			return state;
		}

		[Query(typeof(GetCounter))]
		public static int OnGetCounter([Query(typeof(GetState))] AppStateModel2 state)
		{
			return state.Counter;
		}

		[Query(typeof(GetNames))]
		public static string[] OnGetNames([Query(typeof(GetState))] AppStateModel2 state)
		{
			return state.Names;
		}

		[Query(typeof(GetName))]
		public static string OnGetName(
			[Query(typeof(GetNames))] string[] names,
			[Query(typeof(GetCounter))] int counter)
		{
			return names[counter];
		}

		[Query(typeof(GetConfig))]
		public static IReadOnlyDictionary<string, object> OnGetConfig(
			[Query(typeof(GetState))] AppStateModel2 state)
		{
			return state.KeyValues;
		}

		[Query(typeof(GetCounterSum))]
		public static int OnGetCounterSum(
			[Query(typeof(GetCounter))] int c1,
			[Query(typeof(GetCounter))] int c2,
			[Query(typeof(GetCounter))] int c3,
			[Query(typeof(GetCounter))] int c4,
			[Query(typeof(GetCounter))] int c5,
			[Query(typeof(GetCounter))] int c6,
			[Query(typeof(GetCounter))] int c7)
		{
			return new[] {c1, c2, c3, c4, c5, c6, c7}.Sum();
		}
	}
}
