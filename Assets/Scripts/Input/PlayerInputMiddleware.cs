using System;
using Main;

namespace Input {
	[Serializable]
	public class PlayerInputMiddleware : InputMiddleware<PlayerInputData> {
		public float testFloat;
		
		public override void TransformInput(ref PlayerInputData inputData) {
			inputData = App.InputManager.PlayerInputData;
		}
	}
}