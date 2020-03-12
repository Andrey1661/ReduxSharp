using ReduxSharp.Store.Selectors;

namespace ReduxSharp.TestSet1.Array
{
	public static partial class ArrayQuery
	{
		public class GetState : IQuery<ArrayStateModel> { }
		public class GetNames : IQuery<string[]> { }
		public class GetName : IQuery<string> { }
	}
}
