namespace ReduxSharp.Store.Actions
{
	public interface IAction { }

	public interface IAction<out T>
	{
		T Payload { get; }
	}
}
