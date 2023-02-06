using System;
using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.Middleware.Enemy.Skink {
	public class
		MovementMiddleware : InputMiddleware<SkinkEnemyInput, SkinkEnemyInputEvents.Dispatcher> {
		public float MinDistance = 1.0f;

		public override void TransformInput(ref SkinkEnemyInput inputData) {
			switch (inputData.State) {
				case SkinkEnemyState.Chase:
					Vector3 toPlayer = inputData.PlayerPos - perceptron.transform.position;
					inputData.Movement = toPlayer.magnitude > MinDistance
						                     ? new Vector2(toPlayer.x, toPlayer.z).normalized
						                     : Vector2.zero;

					break;
				case SkinkEnemyState.Idle:
					inputData.Movement = Vector2.zero;
					break;
				default:
					inputData.Movement = Vector2.zero;
					break;
			}
		}

		public override void Init() { }

		public override InputMiddleware<SkinkEnemyInput, SkinkEnemyInputEvents.Dispatcher> Clone() {
			return new MovementMiddleware {MinDistance = MinDistance};
		}
	}
}