using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectile;

    public float shootSpeed = 10f;

    public int count = 5;

    public float randomness = 0.1f;

    public Transform spawnTransform;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        if(UnityEngine.Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < count; i++)
            {
                GameObject instantiatedProjectile = Instantiate(projectile, spawnTransform.position, spawnTransform.rotation);
                Rigidbody rigidbody = instantiatedProjectile.GetComponent<Rigidbody>();
                rigidbody.AddForce((spawnTransform.forward + new Vector3(UnityEngine.Random.Range(-randomness, randomness), UnityEngine.Random.Range(-randomness, randomness), UnityEngine.Random.Range(-randomness, randomness))) * shootSpeed, ForceMode.Impulse);
            }
            
        }
    }
}
