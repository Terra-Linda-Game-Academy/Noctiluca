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

		private void Awake() {
			_inputActions = new InputActions();
			_inputActions.Enable();

			_inputActions.InGame.Aim.performed += ctx => {
				                                      playerInput.Aim = ctx.ReadValue<Vector2>();
				                                      //todo: makes gamepad the default, might not be a good idea
				                                      //todo: but i cant test b/c in baker rn :(
				                                      if (ctx.control.path == "/Mouse/position") {
					                                      playerInput.Control = PlayerInput.ControlType.KeyboardMouse;
				                                      } else { playerInput.Control = PlayerInput.ControlType.Gamepad; }
			                                      };
			_inputActions.InGame.Aim.canceled       += _ => { playerInput.Aim        = Vector2.zero; };
			_inputActions.InGame.Movement.performed += ctx => { playerInput.Movement = ctx.ReadValue<Vector2>(); };
			_inputActions.InGame.Movement.canceled  += _ => { playerInput.Movement   = Vector2.zero; };
			_inputActions.InGame.Interact.performed += _ => { OnInteract?.Invoke(); };
			_inputActions.InGame.Attack.performed   += _ => { OnAttack?.Invoke(); };
			_inputActions.InGame.Throw.performed    += _ => { OnThrow?.Invoke(); };
		}
	}
}