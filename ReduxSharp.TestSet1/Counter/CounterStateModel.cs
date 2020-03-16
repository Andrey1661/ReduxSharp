using System;
using ReduxSharp.Store;

namespace ReduxSharp.TestSet1.Counter
{
	public class CounterStateModel : ICloneable<CounterStateModel>
	{
		public CounterStateModel()
		{
			Value = 0;
		}

		public CounterStateModel(int value)
		{
			Value = value;
		}

		public int Value { get; }

		public CounterStateModel Clone()
		{
			return new CounterStateModel(Value);
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}
