using System.Linq;
using ReduxSharp.Store.Actions;
using ReduxSharp.Store.Attributes;

namespace ReduxSharp.TestSet1.Array
{
	[State(typeof(ArrayStateModel))]
	public partial class ArrayState
	{
		[Action(typeof(SetNames))]
		public void OnSetNames(IStateContext<ArrayStateModel> context, SetNames action)
		{
			context.SetState(new ArrayStateModel(action.Payload.ToArray()));
		}
	}
}
