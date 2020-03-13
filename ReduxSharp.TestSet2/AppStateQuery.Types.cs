using System.Collections.Generic;
using ReduxSharp.Store.Selectors;

namespace ReduxSharp.TestSet2
{
	public static partial class AppStateQuery
	{
		public class GetState : IQuery<AppStateModel2> { }
		public class GetCounter : IQuery<int> { }
		public class GetNames : IQuery<string[]> { }
		public class GetName : IQuery<string> { }
		public class GetConfig : IQuery<IReadOnlyDictionary<string, object>> { }
	}
}
