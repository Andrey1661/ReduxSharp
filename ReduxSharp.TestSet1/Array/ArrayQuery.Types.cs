using ReduxSharp.Store.Selectors;

namespace ReduxSharp.TestSet1.Array
{
	public static partial class ArrayQuery
	{
		public class GetState : Query<GetState, ArrayStateModel> { }
		public class GetNames : Query<GetNames, string[]> { }
		public class GetName : Query<GetName, string> { }
	}
}
