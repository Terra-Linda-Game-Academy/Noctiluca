using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;
using Util;

namespace Input.Middleware.Enemy.Skink {
	public class
		PerceptionMiddleware : InputMiddleware<SkinkEnemyInput, SkinkEnemyInputEvents.Dispatcher> {
		public float MaxViewDistance = 20.0f;
		public float MaxViewAngle    = 60.0f;

		public ScriptableVar<MonoBehaviour> Player;

		public override void TransformInput(ref SkinkEnemyInput inputData) {
			if (perceptron.VisionCone(Player.Value.gameObject, MaxViewDistance, MaxViewAngle / 2,
			                          ~LayerMask.GetMask("Player"))) {
				inputData.PlayerPos = Player.Value.transform.position;
				inputData.State     = SkinkEnemyState.Chase;
			} else { inputData.State = SkinkEnemyState.Idle; }
		}

		public override void Init() { }

		public override InputMiddleware<SkinkEnemyInput, SkinkEnemyInputEvents.Dispatcher> Clone() {
			return new PerceptionMiddleware {
				                                            MaxViewDistance = MaxViewDistance,
				                                            MaxViewAngle    = MaxViewAngle,
				                                            Player          = Player
			                                            };
		}
	}
}