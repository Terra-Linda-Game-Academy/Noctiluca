using System;
using Input.Events;
using Main;

namespace Input {
	[Serializable]
	public class PlayerInputSystemMiddleware : InputMiddleware<PlayerInputData, PlayerInputEvents.Dispatcher> {
		public float testFloat;

		// ReSharper disable once RedundantAssignment
		public override void TransformInput(ref PlayerInputData inputData) {
			inputData = App.InputManager.playerInputData;
		}

		public override void Init() {
			App.InputManager.OnInteract += Dispatcher.Interact;
		}
	}
}