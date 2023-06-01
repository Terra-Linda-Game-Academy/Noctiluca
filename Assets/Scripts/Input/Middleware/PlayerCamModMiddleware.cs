using System;
using Input.Data;
using Input.Events;
using UnityEngine;

namespace Input.Middleware {
	[Serializable]
	public class PlayerCamModMiddleware : InputMiddleware<PlayerInput, PlayerInputEvents.Dispatcher> {
		private UnityEngine.Camera _camera;

		public override void TransformInput(ref PlayerInput inputData) {
			Quaternion rotationMod = Quaternion.Euler(0f, _camera.transform.rotation.eulerAngles.y, 0f);

			//movement
			Vector3 rotatedMovement = rotationMod * new Vector3(inputData.Movement.x, 0f, inputData.Movement.y);
			inputData.Movement = new Vector2(rotatedMovement.x, rotatedMovement.z);

			//aim
			if (inputData.Control == PlayerInput.ControlType.Gamepad) {
				Vector3 rotatedAim = rotationMod * new Vector3(inputData.Aim.x, 0f, inputData.Aim.y);
				inputData.Aim = new Vector2(rotatedAim.x, rotatedAim.z);
			} else {
				Ray ray = _camera.ScreenPointToRay(inputData.Aim);

				if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Raycast Floor"))) {
					Vector3 playerToPoint = (hit.point - perceptron.transform.position).normalized;
					inputData.Aim = new Vector2(playerToPoint.x, playerToPoint.z);
				} else { inputData.Aim = Vector2.zero; }
			}
		}

		public override void Init() { _camera = UnityEngine.Camera.main; }

		public override InputMiddleware<PlayerInput, PlayerInputEvents.Dispatcher> Clone() {
			return new PlayerCamModMiddleware {_camera = _camera};
		}
	}
}