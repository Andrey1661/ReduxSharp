using System;

namespace ReduxSharp.Store.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
	public class QueryAttribute : Attribute
	{
		public Type MarkerType { get; }

		public QueryAttribute(Type markerType)
		{
			MarkerType = markerType;
		}
	}
}
