using ReduxSharp.Store.Attributes;
using ReduxSharp.TestSet1.Counter;

namespace ReduxSharp.TestSet1.Array
{
	[Query(typeof(ArrayStateModel))]
	public static partial class ArrayQuery
	{
		[Query(typeof(GetState))]
		public static ArrayStateModel OnGetState(ArrayStateModel state)
		{
			return state;
		}

		[Query(typeof(GetNames))]
		public static string[] OnGetNames([Query(typeof(GetState))] ArrayStateModel state)
		{
			return state.Names;
		}

		[Query(typeof(GetName))]
		public static string OnGetName(
			[Query(typeof(GetNames))] string[] names,
			[Query(typeof(CounterQuery.GetValue))] int index)
		{
			return names[index];
		}
	}
}
