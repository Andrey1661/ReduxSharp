using System;

namespace ReduxSharp.TestSet1.Array
{
	public class ArrayStateModel : ICloneable
	{
		public ArrayStateModel()
		{
			Names = new string[0];
		}

		public ArrayStateModel(string[] names)
		{
			Names = names;
		}

		public string[] Names { get; }

		public object Clone()
		{
			return new ArrayStateModel(Names);
		}
	}
}
