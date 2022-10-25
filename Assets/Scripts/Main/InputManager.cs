using Input;
using UnityEngine;

namespace Main {
    public class InputManager : MonoBehaviour {
        private InputActions _inputActions;

        public PlayerInputData PlayerInputData;

        private void Awake() {
            _inputActions = new InputActions();
            _inputActions.Enable();

            _inputActions.InGame.Aim.performed += ctx => {
                                                      PlayerInputData.aim = ctx.ReadValue<Vector2>();
                                                  };
            _inputActions.InGame.Aim.canceled += _ => {
                                                     PlayerInputData.aim = Vector2.zero;
                                                 };
            _inputActions.InGame.Movement.performed += ctx => {
                                                      PlayerInputData.movement = ctx.ReadValue<Vector2>();
                                                  };
            _inputActions.InGame.Movement.canceled += _ => {
                                                     PlayerInputData.movement = Vector2.zero;
                                                 };
        }
    }
}