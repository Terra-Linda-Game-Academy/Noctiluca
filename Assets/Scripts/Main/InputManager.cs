using Input;
using Input.Data;
using System;
using UnityEngine;

namespace Main
{
    public class InputManager : MonoBehaviour
    {
        private InputActions _inputActions;

        private PlayerInput playerInput;

        public PlayerInput PlayerInput => playerInput;

        public Action OnInteract;

        private void Awake()
        {
            _inputActions = new InputActions();
            _inputActions.Enable();

            _inputActions.InGame.Aim.performed += ctx => { playerInput.Aim = ctx.ReadValue<Vector2>(); };
            _inputActions.InGame.Aim.canceled += _ => { playerInput.Aim = Vector2.zero; };
            _inputActions.InGame.Movement.performed += ctx => { playerInput.Movement = ctx.ReadValue<Vector2>(); };
            _inputActions.InGame.Movement.canceled += _ => { playerInput.Movement = Vector2.zero; };
            _inputActions.InGame.Interact.performed += _ => { OnInteract?.Invoke(); };
        }
    }
}