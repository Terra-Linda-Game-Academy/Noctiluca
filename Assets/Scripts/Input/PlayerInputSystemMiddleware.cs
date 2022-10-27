using System;
using Main;

namespace Input {
	[Serializable]
	public class PlayerInputSystemMiddleware : PlayerInputMiddleware {
		public float testFloat;
		
		public override void TransformInput(ref PlayerInputData inputData) {
			inputData = App.InputManager.PlayerInputData;
		}
	}
}