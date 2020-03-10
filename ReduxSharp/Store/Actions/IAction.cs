namespace ReduxSharp.Store.Actions
{
	public interface IAction<T>
	{
		T Payload { get; }
	}
}
