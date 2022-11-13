using System;
using Input;
using UnityEngine;

namespace Main {
	public class InputManager : MonoBehaviour {
		private InputActions _inputActions;

		[HideInInspector] public PlayerInput playerInput;

		public Action OnInteract;

		private void Awake() {
			_inputActions = new InputActions();
			_inputActions.Enable();

			_inputActions.InGame.Aim.performed      += ctx => { playerInput.aim      = ctx.ReadValue<Vector2>(); };
			_inputActions.InGame.Aim.canceled       += _ => { playerInput.aim        = Vector2.zero; };
			_inputActions.InGame.Movement.performed += ctx => { playerInput.movement = ctx.ReadValue<Vector2>(); };
			_inputActions.InGame.Movement.canceled  += _ => { playerInput.movement   = Vector2.zero; };

			_inputActions.InGame.Interact.performed += _ => { OnInteract?.Invoke(); };
		}
	}
}