using UnityEngine;
using Util;

namespace AI {
	public class StateController : MonoBehaviour {
		public bool  aiActive;
		
		public State      currentState;
		public EnemyStats stats;

		public Transform eyes;

		public Option<float> testFloat;

		[HideInInspector] public Transform target;

		private void Update() {
			if (!aiActive) return;
			
			currentState.UpdateState(this);
		}

		private void OnDrawGizmos() {
			if (currentState != null && stats != null) {
				Gizmos.color = currentState.gizmoColor;
				Gizmos.DrawWireSphere(eyes.position, stats.lookRadius);
			}
		}
	}
}