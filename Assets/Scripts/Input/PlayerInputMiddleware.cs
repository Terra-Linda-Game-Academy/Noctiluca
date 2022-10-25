using System;
using Main;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Input {
	[Serializable]
	public class PlayerInputMiddleware : InputMiddleware<PlayerInputData> {
		public float testFloat;
		
		public override void TransformInput(ref PlayerInputData inputData) {
			inputData = App.InputManager.PlayerInputData;
		}

		public override VisualElement Body() {
			FloatField floatField = new FloatField("testFloat") {bindingPath = "testFloat", style = { flexGrow = 1}};
			return floatField;
		}
	}
}