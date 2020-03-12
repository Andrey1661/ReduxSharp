using System.Collections.Generic;
using ReduxSharp.Store.Actions;

namespace ReduxSharp.TestSet1.Array
{
	public partial class ArrayState
	{
		public class SetNames : IAction<IEnumerable<string>>
		{
			public SetNames(IEnumerable<string> names)
			{
				Payload = names;
			}

			public IEnumerable<string> Payload { get; }
		}
	}
}
