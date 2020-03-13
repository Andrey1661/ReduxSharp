using System;
using System.Collections.Generic;
using System.Text;
using ReduxSharp.Store.Actions;

namespace ReduxSharp.TestSet2
{
	public partial class AppState
	{
		public class SetCounter : IAction<int>
		{
			public SetCounter(int payload)
			{
				Payload = payload;
			}

			public int Payload { get; }
		}

		public class Increment : IAction { }
		public class Decrement : IAction { }

		public class SetNames : IAction<string[]> 
		{
			public SetNames(string[] payload)
			{
				Payload = payload;
			}

			public string[] Payload { get; }
		}

		public class SetConfig : IAction<IReadOnlyDictionary<string, object>>
		{
			public SetConfig(IReadOnlyDictionary<string, object> payload)
			{
				Payload = payload;
			}

			public IReadOnlyDictionary<string, object> Payload { get; }
		}
	}
}
