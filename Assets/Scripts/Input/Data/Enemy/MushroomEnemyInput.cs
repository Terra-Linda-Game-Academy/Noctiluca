using AI;
using UnityEngine;

namespace Input.Data.Enemy {
	public struct MushroomEnemyInput : IInputBlockable {
		public Vector2 Movement;
		public Vector3 PlayerPos;

		public WalkingEnemyState State;

		public void Block() { Movement = Vector2.zero; }
	}
}