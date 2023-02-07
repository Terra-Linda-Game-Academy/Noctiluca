using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;
using Util;

namespace Input.Middleware.Enemy.Mushroom {
	public class
		PerceptionMiddleware : InputMiddleware<MushroomEnemyInput, MushroomEnemyInputEvents.Dispatcher> {
		public float MaxViewDistance = 20.0f;
		public float MaxViewAngle    = 60.0f;

		public ScriptableVar<MonoBehaviour> Player;

		public override void TransformInput(ref MushroomEnemyInput inputData) {
			if (perceptron.VisionCone(Player.Value.gameObject, MaxViewDistance, MaxViewAngle / 2,
			                          ~LayerMask.GetMask("Player"))) {
				inputData.PlayerPos = Player.Value.transform.position;
				inputData.State     = MushroomEnemyStates.Chase;
			} else { inputData.State = MushroomEnemyStates.Wander; }
		}

		public override void Init() { }

		public override InputMiddleware<MushroomEnemyInput, MushroomEnemyInputEvents.Dispatcher> Clone() {
			return new PerceptionMiddleware {
				                                MaxViewDistance = MaxViewDistance,
				                                MaxViewAngle    = MaxViewAngle,
				                                Player          = Player
			                                };
		}
	}
}