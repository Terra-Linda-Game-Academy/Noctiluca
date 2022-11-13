using System;
using Input.Events;
using Main;

namespace Input {
	[Serializable]
	public class PlayerInputMiddleware : InputMiddleware<PlayerInput, PlayerInputEvents.Dispatcher> {
		public override void TransformInput(ref PlayerInput input) {
			input = App.InputManager.playerInput;
		}

		public override void Init() {
			App.InputManager.OnInteract += Dispatcher.Interact;
		}
	}
}