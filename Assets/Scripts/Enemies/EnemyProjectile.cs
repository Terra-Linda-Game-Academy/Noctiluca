using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyProjectile : MonoBehaviour
{
    public float lifetime;
    public float force;
    Rigidbody rigidbody;
    private void Awake()
    {
        Destroy(gameObject, lifetime);

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * force);
    }
}
