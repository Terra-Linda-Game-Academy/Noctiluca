using Input.ConcreteInputProviders;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float TransformMovement;

    public float hmmm = 5;

    private float speed;
    private bool slow;
    private float sprintspeed;

    public InputActionAsset InputActions;

    private Transform playerTransform;

    public PlayerInputProvider inputProvider;
    void Start()
    {
        InputAction Move = GetComponent<InputAction>();
        playerTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Move(InputAction.CallbackContext context)
    {

    }
}

