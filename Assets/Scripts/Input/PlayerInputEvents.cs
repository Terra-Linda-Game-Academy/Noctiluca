using System;

namespace Input {
	public struct PlayerInputEvents {
		public Action OnJump;
		public Action OnInteract;

		public Action<string> TestAct;
	}
}