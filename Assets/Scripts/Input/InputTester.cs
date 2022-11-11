using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputTester : MonoBehaviour
    {
        private InputActions inputActions;

        // Start is called before the first frame update
        void Start()
        {
            inputActions = new InputActions();
            inputActions.Enable();

            inputActions.InGame.Interact.performed += LogTheThing;
            inputActions.InGame.PotionSwap.performed += LogTheThing;
            inputActions.InGame.PrimaryAttack.performed += LogTheThing;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LogTheThing(InputAction.CallbackContext ctx)
        {
            Debug.Log(ctx);
            Debug.Log(ctx.control.device);
        }

        private void Aiming(InputAction.CallbackContext ctx)
        {
            
        }
    }

}
