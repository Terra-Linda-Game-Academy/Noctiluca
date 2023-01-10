using UnityEngine;

namespace Input.Data.Enemy {
	public struct WalkingEnemyInput : IInputBlockable {
		public Vector2 Movement;

		public Vector3 PlayerPos;

		public void Block() { Movement = Vector2.zero; }
	}
}