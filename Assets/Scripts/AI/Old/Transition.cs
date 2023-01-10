using Util;

namespace AI.Old {
	[System.Serializable]
	public struct Transition {
		public Decision decision;

		public State         trueState;
		public Option<State> falseState;
	}
}