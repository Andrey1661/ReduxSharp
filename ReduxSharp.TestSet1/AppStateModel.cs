﻿using System;
using ReduxSharp.Store;
using ReduxSharp.TestSet1.Array;
using ReduxSharp.TestSet1.Config;
using ReduxSharp.TestSet1.Counter;

namespace ReduxSharp.TestSet1
{
	public class AppStateModel1 : ICloneable<AppStateModel1>
	{
		public AppStateModel1()
		{
			Value = null;
			Counter = new CounterStateModel();
			Array = new ArrayStateModel();
			Config = new ConfigStateModel();
		}

		public AppStateModel1(CounterStateModel counter, ArrayStateModel array, ConfigStateModel config)
		{
			Counter = counter;
			Array = array;
			Config = config;
		}

		public CounterStateModel Counter { get; }
		public ArrayStateModel Array { get; }
		public ConfigStateModel Config { get; }
		public string Value { get; }

		public AppStateModel1 Clone()
		{
			return new AppStateModel1(Counter, Array, Config);
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}
