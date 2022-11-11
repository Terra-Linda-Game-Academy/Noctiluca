using System;
using Main;

namespace Input {
	[Serializable]
	public class PlayerInputSystemMiddleware : InputMiddleware<PlayerInputData, PlayerInputEvents> {
		public float testFloat;

		public override void TransformInput(ref PlayerInputData inputData, ref PlayerInputEvents events) {
			inputData = App.InputManager.playerInputData;
			Events    = events;
		}

		protected override void EventSubscriptions() {
			ProviderTester.TestAction += TestAct;
		}

		protected override void DisposeSubscriptions() {
			ProviderTester.TestAction -= TestAct;
		}

		private void TestAct() {
			Events.TestAct?.Invoke($"{testFloat}");
		}
	}
}