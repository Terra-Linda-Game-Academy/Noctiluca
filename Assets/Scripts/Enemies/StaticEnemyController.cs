using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyController : MonoBehaviour
{
    public float range;
    public float cooldown;
    public GameObject player;
    public GameObject projectilePrefab;
    private bool shooting;
    void Update()
    {
        if(Vector3.Distance(player.transform.position, gameObject.transform.position) <= range)
        {
            transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

            if(!shooting)
                StartCoroutine("Shoot");
        }
    }

    IEnumerator Shoot()
    {
        shooting = true;
        Instantiate(projectilePrefab, transform.position + 1f * transform.forward, transform.rotation);
        yield return new WaitForSeconds(cooldown);
        shooting = false;
    }
}
