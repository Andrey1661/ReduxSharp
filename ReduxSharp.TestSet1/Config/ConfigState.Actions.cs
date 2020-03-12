using System.Collections.Generic;
using ReduxSharp.Store.Actions;

namespace ReduxSharp.TestSet1.Config
{
	public partial class ConfigState
	{
		public class SetConfig : IAction<IEnumerable<KeyValuePair<string, object>>>
		{
			public SetConfig(IEnumerable<KeyValuePair<string, object>> payload)
			{
				Payload = payload;
			}

			public IEnumerable<KeyValuePair<string, object>> Payload { get; }
		}
	}
}
