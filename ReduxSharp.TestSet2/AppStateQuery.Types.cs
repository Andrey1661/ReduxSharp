using System.Collections.Generic;
using ReduxSharp.Store.Selectors;

namespace ReduxSharp.TestSet2
{
	public static partial class AppStateQuery
	{
		public class GetState : Query<GetState, AppStateModel2> { }
		public class GetCounter : Query<GetCounter, int> { }
		public class GetNames : Query<GetNames, string[]> { }
		public class GetName : Query<GetName, string> { }
		public class GetConfig : Query<GetConfig, IReadOnlyDictionary<string, object>> { }
		public class GetCounterSum : Query<GetCounterSum, int> { }
	}
}
