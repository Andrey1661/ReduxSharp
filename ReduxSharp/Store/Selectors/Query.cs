namespace ReduxSharp.Store.Selectors
{
	// ReSharper disable once UnusedTypeParameter
	public interface IQuery<TReturn> { }

	public abstract class Query<TSelf, TReturn> : IQuery<TReturn>
		where TSelf : Query<TSelf, TReturn>, new()
	{
		private static Query<TSelf, TReturn> _instance;

		public static Query<TSelf, TReturn> Default => _instance ?? (_instance = new TSelf());
	}
}
