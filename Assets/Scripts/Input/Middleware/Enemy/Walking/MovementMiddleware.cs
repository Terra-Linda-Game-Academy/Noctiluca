using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.Middleware.Enemy.Walking {
	public class
		MovementMiddleware : InputMiddleware<WalkingEnemyInput, WalkingEnemyInputEvents.Dispatcher> {
		public float MinDistance = 1.0f;

		public float AttackCooldown = 2.0f;

		private float _attackCountDown;

		public override void TransformInput(ref WalkingEnemyInput inputData) {
			switch (inputData.State) {
				case WalkingEnemyState.Chase:
					Vector3 toPlayer = inputData.PlayerPos - perceptron.transform.position;
					if (toPlayer.magnitude > MinDistance) {
						inputData.Movement = new Vector2(toPlayer.x, toPlayer.z).normalized;
					} else {
						inputData.Movement = Vector2.zero;
						TryAttack();
					}
					break;
				case WalkingEnemyState.Idle:
					inputData.Movement = Vector2.zero;
					break;
				default:
					inputData.Movement = Vector2.zero;
					break;
			}
		}

		private void TryAttack() {
			if (_attackCountDown <= 0) {
				Dispatcher.Attack();
				_attackCountDown = AttackCooldown;
			}

			_attackCountDown -= 1 / 50f;
		}

		public override void Init() { }

		public override InputMiddleware<WalkingEnemyInput, WalkingEnemyInputEvents.Dispatcher> Clone() {
			return new MovementMiddleware {MinDistance = MinDistance};
		}
	}
}