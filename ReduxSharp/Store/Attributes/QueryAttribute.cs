using System;

namespace ReduxSharp.Store.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class QueryAttribute : Attribute
	{
		public Type MarkerType { get; }
		public Type[] Dependencies { get; }

		public QueryAttribute(Type markerType, params Type[] dependencies)
		{
			MarkerType = markerType;
			Dependencies = dependencies;
		}
	}
}
