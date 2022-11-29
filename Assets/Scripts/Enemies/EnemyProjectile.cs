using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime;
    private void Awake()
    {
        Destroy(gameObject, lifetime);
    }

    public void Update()
    {
        transform.position += Vector3.forward * Time.deltaTime;
    }
}
