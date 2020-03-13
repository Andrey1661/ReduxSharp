using System.Collections.Generic;
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
	}
}
