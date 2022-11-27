using UnityEngine;

namespace AI {
	[CreateAssetMenu(menuName = "AI/State")]
	public class State : ScriptableObject {
		public Action[]     actions;
		public Transition[] transitions;

		public Color gizmoColor = Color.gray;

		public void UpdateState(StateController controller) {
			PerformActions(controller);
			CheckTransitions(controller);
		}

		private void PerformActions(StateController controller) {
			foreach (Action action in actions) { action.Act(controller); }
		}

		private void CheckTransitions(StateController controller) {
			foreach (Transition transition in transitions) {
				if (transition.decision.Decide(controller)) {
					controller.currentState = transition.trueState;
					return;
				}

				if (transition.falseState.Enabled) {
					controller.currentState = transition.falseState.Value;
					return;
				}
			}
		}
	}
}