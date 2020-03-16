using System;

namespace ReduxSharp.Store
{
	public interface ICloneable<out T> : ICloneable
	{
		new T Clone();
	}
}
