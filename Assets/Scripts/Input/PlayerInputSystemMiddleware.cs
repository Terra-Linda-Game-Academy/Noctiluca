using System;
using Main;

namespace Input {
	[Serializable]
	public class PlayerInputSystemMiddleware : InputMiddleware<PlayerInputData, PlayerInputEvents> {
		public float testFloat;

		// ReSharper disable once RedundantAssignment
		public override void TransformInput(ref PlayerInputData inputData, ref PlayerInputEvents events) {
			inputData = App.InputManager.playerInputData;
			Events    = events;
		}

		protected override void EventSubscriptions() {
			//subscribe events
		}

		protected override void DisposeSubscriptions() {
			//unsubscribe events
		}
	}
}