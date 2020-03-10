using ReduxSharp.Store.Attributes;
using ReduxSharp.Tests.Model;

namespace ReduxSharp.Tests
{
	[Query(typeof(AppStateModel))]
	public static class AppStateQuery
	{
		[Query(typeof(GetState))]
		public static AppStateModel GetState(AppStateModel state)
		{
			return state;
		}

		[Query(typeof(GetCounter), typeof(GetState))]
		public static int GetCounter(AppStateModel state)
		{
			return state.Counter;
		}

		[Query(typeof(GetSubState), typeof(GetState))]
		public static SubStateModel GetSubState(AppStateModel state)
		{
			return state.SubState;
		}

		[Query(typeof(GetSubStateName), typeof(GetSubState), typeof(GetCounter))]
		public static string GetSubStateName(SubStateModel state, int index)
		{
			return state.Names[index];
		}
	}
}
