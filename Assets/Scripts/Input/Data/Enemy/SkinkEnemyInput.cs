using AI;
using UnityEngine;

namespace Input.Data.Enemy {
	public struct SkinkEnemyInput : IInputBlockable {
		public Vector2 Movement;
		public Vector3 PlayerPos;

		public SkinkEnemyState State;

		public void Block() { Movement = Vector2.zero; }
	}
}