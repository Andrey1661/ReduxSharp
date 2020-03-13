using System.Collections.Generic;
using ReduxSharp.Store.Actions;
using ReduxSharp.Store.Attributes;

namespace ReduxSharp.TestSet1.Config
{
	[State(typeof(ConfigStateModel))]
	public partial class ConfigState
	{
		[Action(typeof(SetConfig))]
		public void OnSetConfig(IStateContext<ConfigStateModel> context, SetConfig action)
		{
			var state = new ConfigStateModel(new Dictionary<string, object>(action.Payload));
			context.SetState(state);
		}
	}
}
