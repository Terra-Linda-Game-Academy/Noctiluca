using Input.ConcreteInputProviders;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    /// <summary>
    /// Author : EvanisEpicandCool
    /// 
    /// Description : And I will cast abominable filth upon thee,
    ///                         and make thee vile,
    ///                and will set thee as a gazingstock.
    ///                                                 -Nahum 3:6
    ///                                                 
    /// Simple Character/Player Controller to take in external inputmap modifiers
    /// also has some Movement code that is on a string
    /// 
    /// Date : 11/9/22
    /// 
    /// </summary>

    //Just a Couple Movement paramaters
    [SerializeField] private float movementSpeed = 3;
    [SerializeField] private float rotationSpeed = 10;

    //inputActions and Frungle
    [HideInInspector] private InputAction movement;
    [SerializeField] private InputActionAsset frungle;


    //Movement Vectors and Bools
    private bool IsMoving => Direction != Vector3.zero;
    private Vector3 Direction { get; set; }
    private Quaternion Rotation => Quaternion.LookRotation(RotationDirection);
    private Vector3 RotationDirection => Vector3.RotateTowards(transform.forward, Direction, rotationSpeed * Time.deltaTime, 0);


    private void Awake()
    {
        //Find The Action and set it to the Pre-Coded movement inputaction
        var gameplayActionMap = frungle.FindActionMap("In Game");

        movement = gameplayActionMap.FindAction("Movement");

        gameObject.AddComponent<Rigidbody>();

        movement.performed += OnMovementChanged;
        movement.canceled += OnMovementChanged;
    }

    private void OnMovementChanged(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        Direction = new Vector3(direction.x, 0, direction.y);
    }

    private void FixedUpdate()
    {
        if (!IsMoving) return;

        transform.position += Direction * movementSpeed * Time.deltaTime;
        transform.rotation = Rotation;
    }


    private void OnDisable()
    {
        movement.Disable();
    }

    private void OnEnable()
    {
        movement.Enable();
    }
}