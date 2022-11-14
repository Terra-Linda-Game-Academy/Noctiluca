using System;
using Input.Data;
using Input.Events;
using Main;

namespace Input.Middleware {
	[Serializable]
	public class PlayerInputMiddleware : InputMiddleware<PlayerInput, PlayerInputEvents.Dispatcher> {
		public override PlayerInput TransformInput(PlayerInput input) => App.InputManager.playerInput;
		
		public override void Init() {
			App.InputManager.OnInteract += Dispatcher.Interact;
		}

		/*public override void Release() {
			App.InputManager.OnInteract -= Dispatcher.Interact;
		}*/
	}
}