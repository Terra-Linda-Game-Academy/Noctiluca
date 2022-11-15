using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.AI;

public class PotatoMob : MonoBehaviour
{

    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    bool isWandering = true;
    bool triggered = false;

    public GameObject player;

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
        if(!triggered)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 3f)
            {
                triggered = true;
                isWandering = false;
                RunToBurrow();
            }
        }

        if (triggered && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    model.transform.position = this.transform.position - new Vector3(0, 0.6f, 0);
                }
            }
        }

    }

    public void RunToBurrow()
    {
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
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

