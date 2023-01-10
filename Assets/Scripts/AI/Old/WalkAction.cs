using UnityEngine;

namespace AI.Old {
	[CreateAssetMenu(menuName = "AI/Actions/Walk")]
	public class WalkAction : Action {
		public float speed = 0.1f;

		public override void Act(StateController controller) { Walk(controller); }

		private void Walk(StateController controller) {
			controller.transform.position += controller.transform.forward * speed;
		}
	}
}