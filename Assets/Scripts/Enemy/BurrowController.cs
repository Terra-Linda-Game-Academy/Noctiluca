using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BurrowController : MonoBehaviour{

    public NavMeshAgent burrowAgent;
    public GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        burrowAgent = GetComponent<NavMeshAgent>();
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        if ((player.transform.position - this.transform.position).sqrMagnitude < 3 * 3)
        {

            StartCoroutine("Burrow");

        }
    }

    IEnumerable Burrow()
    {
        while(true)
        {
            transform.SetPositionAndRotation(RandomNavmeshLocation(4f), Quaternion.Euler(0, 0, 0));

            transform.LookAt(player.transform);

            yield return new WaitForSeconds(4f);
        }
        
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;

        randomDirection += transform.position;

        NavMeshHit hit;

        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))

        {
            finalPosition = hit.position;
        }


        return finalPosition;
    }

}
