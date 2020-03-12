using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReduxSharp.TestSet1.Config
{
	public class ConfigStateModel : ICloneable
	{
		public ConfigStateModel()
		{
			KeyValues = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());
		}

		public ConfigStateModel(IDictionary<string, object> keyValues)
		{
			KeyValues = new ReadOnlyDictionary<string, object>(keyValues);
		}

		public ReadOnlyDictionary<string, object> KeyValues { get; }

		public object Clone()
		{
			return new ConfigStateModel(KeyValues);
		}
	}
}
