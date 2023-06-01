using UnityEngine;

namespace Input.Data
{
    public struct PlayerInput : IInputBlockable
    {
        public Vector2 Aim;
        public Vector2 Movement;
		public ControlType Control;

		public void Block() {
			Aim = Vector2.zero;
			Movement = Vector2.zero;
		}

		public enum ControlType {
			KeyboardMouse,
			Gamepad
		}
	}
}