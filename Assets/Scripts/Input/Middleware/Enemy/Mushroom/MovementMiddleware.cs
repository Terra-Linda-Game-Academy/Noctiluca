using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.Middleware.Enemy.Mushroom {
	public class
		MovementMiddleware : InputMiddleware<MushroomEnemyInput, MushroomEnemyInputEvents.Dispatcher> {
		public float MinDistance = 1.0f;

		public override void TransformInput(ref MushroomEnemyInput inputData) {
			switch (inputData.State) {
				case WalkingEnemyState.Chase:
					Vector3 toPlayer = inputData.PlayerPos - perceptron.transform.position;
					inputData.Movement = toPlayer.magnitude > MinDistance
						                     ? new Vector2(toPlayer.x, toPlayer.z).normalized
						                     : Vector2.zero;

					break;
				case WalkingEnemyState.Idle:
					inputData.Movement = Vector2.zero;
					break;
				default:
					inputData.Movement = Vector2.zero;
					break;

				// possible / alternative case in the future: wander
			}
		}

		public override void Init() { }

		public override InputMiddleware<MushroomEnemyInput, MushroomEnemyInputEvents.Dispatcher> Clone() {
			return new MovementMiddleware {MinDistance = MinDistance};
		}
	}
}