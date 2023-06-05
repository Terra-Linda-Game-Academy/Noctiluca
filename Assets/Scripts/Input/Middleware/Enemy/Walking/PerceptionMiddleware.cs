using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;
using Util;

namespace Input.Middleware.Enemy.Walking {
	public class
		PerceptionMiddleware : InputMiddleware<WalkingEnemyInput, WalkingEnemyInputEvents.Dispatcher> {
		public float MaxViewDistance = 20.0f;
		public float MaxViewAngle    = 60.0f;

		public ScriptableVar<MonoBehaviour> Player;

		public override void TransformInput(ref WalkingEnemyInput inputData) {
			if (perceptron.VisionCone(Player.Value.gameObject, MaxViewDistance, MaxViewAngle / 2,
			                          ~LayerMask.GetMask("Player", "Ignore Raycast"))) {
				inputData.PlayerPos = Player.Value.transform.position;
				inputData.State     = WalkingEnemyState.Chase;
			} else { inputData.State = WalkingEnemyState.Idle; }
		}

		public override void Init() { }

		public override InputMiddleware<WalkingEnemyInput, WalkingEnemyInputEvents.Dispatcher> Clone() {
			return new PerceptionMiddleware {
				                                MaxViewDistance = MaxViewDistance,
				                                MaxViewAngle    = MaxViewAngle,
				                                Player          = Player
			                                };
		}
	}
}