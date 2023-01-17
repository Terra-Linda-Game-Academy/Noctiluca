using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    public float weight;
    
    public Vector3 offset;
    public bool obeysGravity;
    public GameObject prefab;
    Rigidbody rig;

    // Start is called before the first frame update
    private void Awake()
    {
        offset = transform.position;   
    }

    private void Update()
    {
    }
    public void shoot()
    {
        prefab.AddComponent<Rigidbody>();
        rig = prefab.GetComponent<Rigidbody>();

        Instantiate(prefab, offset, Quaternion.identity);

        rig.AddRelativeForce(new Vector3(0, speed, 0));
        
        if (obeysGravity)
        {
            rig.useGravity = true;
            rig.mass = weight;
        }
        else
        {
            rig.isKinematic = true;
        }
        
    }
}
