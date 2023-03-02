using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Burrower
{
    public class BurrowerEnemyController : MonoBehaviour
    {
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
            StartCoroutine(nameof(Burrow));
        }

        private Vector3 RandomNavmeshLocation(float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;

            randomDirection += transform.position;

            NavMeshHit hit;

            Vector3 finalPosition = Vector3.zero;

            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) { finalPosition = hit.position; }


            return finalPosition;
        }


        IEnumerator Burrow()
        {
            while (true)
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
                Vector3 rotation = projectileSpawn.rotation.eulerAngles;
                Quaternion qrotation = transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, qrotation);
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(),
                                        projectileSpawn.parent.GetComponent<Collider>());

                yield return new WaitForSeconds(1f);
            }
        }
    }
}