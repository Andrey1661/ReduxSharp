using System;
using System.Collections.Generic;
using ReduxSharp.Store;

namespace ReduxSharp.TestSet2
{
	public class AppStateModel2 : ICloneable<AppStateModel2>
	{
		public AppStateModel2()
		{
			Counter = 0;
			Names = new string[0];
			KeyValues = new Dictionary<string, object>();
		}

		public AppStateModel2(int counter, string[] names, IReadOnlyDictionary<string, object> keyValues)
		{
			Counter = counter;
			Names = names;
			KeyValues = keyValues;
		}

		public int Counter { get; }
		public string[] Names { get; }
		public IReadOnlyDictionary<string, object> KeyValues { get; }

		public AppStateModel2 Clone()
		{
			return new AppStateModel2(Counter, Names, KeyValues);
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}
