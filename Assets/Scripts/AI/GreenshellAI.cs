using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenshellAI : MonoBehaviour
{
    public float speed = 1;

    public float maxSlope = 30;
    //ramps are a bit of a problem for this enemy type
    //the max slope calculation actually works but physics happens and the
    //enemy does jumps off ramps which are glorious, but unwanted.
    //either come up with a way to keep it on the ground or just make the max slope 5 degrees

    public bool randomStartDirection;

    public Vector2 startDirection;
    private Vector2 direction;

    public Rigidbody rb;

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

        Vector3 velChange = new Vector3(direction.x, 0, direction.y).normalized * speed;
        rb.velocity = new Vector3(velChange.x, rb.velocity.y, velChange.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 norm = collision.GetContact(0).normal;
        if (norm.y <= Mathf.Cos(Mathf.Deg2Rad * maxSlope))
        {
            direction = Vector2.Reflect(direction, new Vector2(norm.x, norm.z).normalized);
        }
    }
}
