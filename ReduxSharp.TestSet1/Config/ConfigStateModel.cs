using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReduxSharp.Store;

namespace ReduxSharp.TestSet1.Config
{
	public class ConfigStateModel : ICloneable<ConfigStateModel>
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

		public ConfigStateModel Clone()
		{
			return new ConfigStateModel(KeyValues);
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}
