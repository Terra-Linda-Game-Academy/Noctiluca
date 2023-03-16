using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoulder : MonoBehaviour
{
    public float knockbackForce = 10f; // the force to apply to the boulder
    public float upwardForce = 2f; // the force to apply upward to the boulder
    public float velocityFallOff = 0.8f; // the percentage of velocity to retain after each bounce

    private Rigidbody rb;

    void Start()
    {
        // get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize()
    {
        // get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    public void Knockback(Vector3 direction)
    {
        // apply the knockback force in the specified direction
        rb.AddForce(direction.normalized * knockbackForce, ForceMode.Impulse);

        // apply the upward force
        rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        // reduce the velocity of the boulder on each bounce
        rb.velocity *= velocityFallOff;
    }
}
