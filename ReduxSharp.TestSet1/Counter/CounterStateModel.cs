using System;

namespace ReduxSharp.TestSet1.Counter
{
	public class CounterStateModel : ICloneable
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

		public object Clone()
		{
			return new CounterStateModel(Value);
		}
	}
}
