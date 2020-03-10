using ReduxSharp.Store.Selectors;
using ReduxSharp.Tests.Model;

namespace ReduxSharp.Tests
{
	public class GetState : SelectorMarker<AppStateModel> { }

	public class GetCounter : SelectorMarker<int> { }

	public class GetSubState : SelectorMarker<SubStateModel> { }

	public class GetSubStateName : SelectorMarker<string> { }
}
