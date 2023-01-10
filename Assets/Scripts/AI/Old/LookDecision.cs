using UnityEngine;

namespace AI.Old {
	[CreateAssetMenu(menuName = "AI/Decisions/Look")]
	public class LookDecision : Decision {
		public override bool Decide(StateController controller) { return Look(controller); }

		private bool Look(StateController controller) {
			Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.stats.lookRange,
			              Color.green);

			if (Physics.SphereCast(controller.eyes.position, controller.stats.lookRadius, controller.eyes.forward,
			                       out var hit, controller.stats.lookRange)
			 && hit.collider.CompareTag("Player")) {
				controller.target = hit.transform;
				return true;
			}

			return false;
		}
	}
}