using UnityEngine;

namespace Input.Data {
	public struct PlayerInput : IInputBlockable {
		public Vector2 aim;
		public Vector2 movement;

		public void Block() {
			aim = Vector2.zero;
			movement = Vector2.zero;
		}
	}
}