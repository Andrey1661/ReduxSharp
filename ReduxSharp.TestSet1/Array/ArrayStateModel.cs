using System;
using ReduxSharp.Store;

namespace ReduxSharp.TestSet1.Array
{
	public class ArrayStateModel : ICloneable<ArrayStateModel>
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

		public ArrayStateModel Clone()
		{
			return new ArrayStateModel(Names);
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}
