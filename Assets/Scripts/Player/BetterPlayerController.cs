using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class BetterPlayerController : MonoBehaviour
{
    
    [SerializeField, Range(0f, 100f)] //How fast the player can change direction, how snappy the control is.
    float maxAcceleration = 10f, maxAirAcceleration = 1f; //acceleration should be more sluggish in the air.
    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;
    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;
    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f; // Threshold to determine if ground below player is a valid to walk on, See: OnValidate()
    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;

    Rigidbody body;
    Vector3 contactNormal;
    Vector3 velocity, desiredVelocity;
    int groundContactCount;
    bool OnGround => groundContactCount > 0;
    bool desiredJump;
    float minGroundDotProduct;
    int jumpPhase;
    
    
    void Awake () {
        //initialize some variables...
        body = GetComponent<Rigidbody>();
        OnValidate();
    }
    
    void OnValidate() { 
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        /* Checks how perpendicular the contact normal is to determine if the player is on the ground or not.
         * By using the cosine value of the maximum angle allowed and the upward direction the `minGroundDotProduct`
         * can be used as a threshold value to determine if the player is on a valid ground surface.
         * OnValidate() is used to update the value when values are changed in the editor.
        */
    }
    
    void Update() 
    {
        // Read the player's input from the horizontal and vertical axes
        Vector2 playerInput= new Vector2(0f, 0f);
        playerInput.x = UnityEngine.Input.GetAxis("Horizontal");
        playerInput.y = UnityEngine.Input.GetAxis("Vertical");
        
        // Limit the magnitude of the player's input vector to 1 to prevent diagonal movement from being faster
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        
        // Convert the 2D player input into a 3D desired velocity vector
        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        
        // Check if the player has pressed the jump button
        desiredJump |= UnityEngine.Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();
        if (desiredJump) {
            desiredJump = false;
            Jump();
        }
        body.velocity = velocity;
        ClearState();
    }

    void UpdateState () {
        velocity = body.velocity;
        if (OnGround) {
            jumpPhase = 0; 
            if (groundContactCount > 1) {
                contactNormal.Normalize();
            }
        }
        else {
            contactNormal = Vector3.up;
        }
    }
    
    // This method adjusts the velocity of the character based on input and the current state.
    void AdjustVelocity () { 
        //Projects the x and z axis to line up with the ground contact plane so that movement follows the slope of the ground.
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;
        
        /*Calculates the current velocity of the player in the x and z directions by taking the dot product of the current
         velocity as well as the ground-aligned x and z axes. I really wish they taught linear algebra in school. */
        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);
        
        //Calculates the maximum acceleration that can occur in one frame based on whether the player is grounded or not.
        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        //Calculates the new velocities by calling math.MoveTowards on the current and desired velocities.
        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);
        
        //Finally applies the new values to the x and z axis to create the new velocity of the player. 
        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    void Jump()
    {
        if (OnGround || jumpPhase < maxAirJumps) {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            float alignedSpeed = Vector3.Dot(velocity, contactNormal);
            if (alignedSpeed > 0f) {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }
            velocity += contactNormal * jumpSpeed;
        }
    }
    
    void ClearState () {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }
    
    Vector3 ProjectOnContactPlane (Vector3 vector) {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
        /* Project planes along contact normal. E. g. player movement x axis is projected along contact normal so
         * that movement follows the slope of the ground instead of clipping into it.
         */
    }
    
    void OnCollisionEnter (Collision collision) {
        EvaluateCollision(collision);
    }

    void OnCollisionStay (Collision collision) {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    { 
        /* I forgot what this method does. Looks like it might be a traversal to add up the contact point normals
         * that are not overly steep to create the contactNormal but your guess is probably better than mine.
         */
        for (int i = 0; i < collision.contactCount; i++) {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minGroundDotProduct) {
                groundContactCount += 1;
                contactNormal += normal;
            }
        }
    }
}
