using System;
using Input;
using UnityEngine;
using PlayerInput = Input.Data.PlayerInput;

namespace Main {
	public class InputManager : MonoBehaviour {
		private InputActions _inputActions;

		private PlayerInput playerInput;

		public PlayerInput PlayerInput => playerInput;

		public event Action OnInteract;
		public event Action OnAttack;
		public event Action OnThrow;
		public event Action OnPotionSwap;

		private void Awake() {
			_inputActions = new InputActions();
			_inputActions.Enable();

			_inputActions.InGame.MouseAim.performed += ctx => {
				                                           Vector2 val = ctx.ReadValue<Vector2>();
				                                           if (val.magnitude < 1) return;
				                                           playerInput.Aim     = val;
				                                           playerInput.Control = PlayerInput.ControlType.KeyboardMouse;
			                                           };
			_inputActions.InGame.GamepadAim.performed += ctx => {
				                                             Vector2 val = ctx.ReadValue<Vector2>();
				                                             playerInput.Aim     = val;
				                                             playerInput.Control = PlayerInput.ControlType.Gamepad;
			                                             };
			_inputActions.InGame.GamepadAim.canceled  += _ => { playerInput.Aim        = Vector2.zero; };
			_inputActions.InGame.Movement.performed   += ctx => { playerInput.Movement = ctx.ReadValue<Vector2>(); };
			_inputActions.InGame.Movement.canceled    += _ => { playerInput.Movement   = Vector2.zero; };
			_inputActions.InGame.Interact.performed   += _ => { OnInteract?.Invoke(); };
			_inputActions.InGame.Attack.performed     += _ => { OnAttack?.Invoke(); };
			_inputActions.InGame.Throw.performed      += _ => { OnThrow?.Invoke(); };
			_inputActions.InGame.PotionSwap.performed += _ => { OnPotionSwap?.Invoke(); };
		}
	}
}