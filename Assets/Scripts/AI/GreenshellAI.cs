using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenshellAI : MonoBehaviour
{
    public float speed = 10;
    public float maxSlope = 45;
    public bool randomStartDirection;
    public Vector2 startDirection;
    public Rigidbody rb;

    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(randomStartDirection)
        {
            direction = Random.insideUnitCircle.normalized;
        } 
        else
        {
            direction = startDirection;
        }
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 newVelocity = new Vector3(direction.x, 0, direction.y) * speed;
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 norm = collision.GetContact(0).normal;
        if (norm.y <= Mathf.Cos(Mathf.Deg2Rad * maxSlope))
        {
            direction = Vector2.Reflect(direction, new Vector2(norm.x, norm.z).normalized);
            Debug.Log(direction);
            direction.Normalize();
        }
    }
}
