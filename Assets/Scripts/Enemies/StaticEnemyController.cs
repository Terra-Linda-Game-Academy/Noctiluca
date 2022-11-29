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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, player.transform.rotation, 100);

            if(!shooting)
                StartCoroutine("Shoot");
        }
    }

    IEnumerator Shoot()
    {
        shooting = true;
        Debug.Log("bang");

        Instantiate(projectilePrefab, gameObject.transform.position, transform.rotation);

        yield return new WaitForSeconds(cooldown);
        shooting = false;
    }
}
