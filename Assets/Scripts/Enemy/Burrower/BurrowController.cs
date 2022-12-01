using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BurrowController : MonoBehaviour {

    public NavMeshAgent burrowAgent;
    public GameObject player;
    public bool playerNear;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public float projectileSpeed;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        burrowAgent = GetComponent<NavMeshAgent>();
        StartCoroutine("Burrow");
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


    IEnumerator Burrow()
    {
        while(true)
        {
            print("teleport");

            StopCoroutine("Attack");

            burrowAgent.Warp(RandomNavmeshLocation(2f));

            transform.LookAt(player.transform);

            yield return new WaitForSeconds(1f);

            StartCoroutine("Attack");

            yield return new WaitForSeconds(2f);
        }

    }

    IEnumerator Attack()
    {
        while (true)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), projectileSpawn.parent.GetComponent<Collider>());
            projectile.transform.position = projectileSpawn.position;
            Vector3 rotation = projectile.transform.rotation.eulerAngles;
            projectile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            projectile.GetComponent<Rigidbody>().AddForce(projectileSpawn.forward * projectileSpeed, ForceMode.Impulse);
            yield return new WaitForSeconds(1f);
        }
        

    }

    private void Update()
    {
    }

}
