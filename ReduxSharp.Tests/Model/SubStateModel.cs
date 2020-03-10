namespace ReduxSharp.Tests.Model
{
	public class SubStateModel
	{
		public SubStateModel()
		{
			Names = new string[0];
		}

		public SubStateModel(string[] names)
		{
			Names = names;
		}

		public string[] Names { get; }
	}
}
