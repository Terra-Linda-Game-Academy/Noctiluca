using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.AI;

public class ChargeMob : MonoBehaviour
{

    public float wanderRadius;
    public float wanderTimer;

    private NavMeshAgent agent;
    private float timer;

    bool isWandering = true;

    public GameObject model;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWandering)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if(hitObject.tag == "Player")
            {
                agent.speed = 100;
                agent.angularSpeed = 0;
                agent.SetDestination(hitObject.transform.position);
            }
            Debug.DrawLine(transform.position, hit.point, Color.cyan);
        }

    }


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }


}

