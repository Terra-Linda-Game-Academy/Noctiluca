using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BasicMob : MonoBehaviour
{

    public float wanderRadius;
    public float wanderTimer;

    protected NavMeshAgent agent;
    protected float timer;

    protected bool isWandering = true;


    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    protected void Awake()
    {
        updateFunctions += wanderAround;
    }

    protected delegate void UpdateFunctions();
    protected UpdateFunctions updateFunctions;

    void Update()
    {
        updateFunctions();
    }

    void wanderAround()
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
