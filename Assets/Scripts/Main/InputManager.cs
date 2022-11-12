using System;
using Input;
using UnityEngine;

namespace Main {
	public class InputManager : MonoBehaviour {
		private InputActions _inputActions;

		[HideInInspector] public PlayerInputData playerInputData;

		public Action OnJump;

		private void Awake() {
			_inputActions = new InputActions();
			_inputActions.Enable();

			_inputActions.InGame.Aim.performed      += ctx => { playerInputData.aim      = ctx.ReadValue<Vector2>(); };
			_inputActions.InGame.Aim.canceled       += _ => { playerInputData.aim        = Vector2.zero; };
			_inputActions.InGame.Movement.performed += ctx => { playerInputData.movement = ctx.ReadValue<Vector2>(); };
			_inputActions.InGame.Movement.canceled  += _ => { playerInputData.movement   = Vector2.zero; };
		}

		private void OnEnable() { OnJump?.Invoke(); }
	}
}