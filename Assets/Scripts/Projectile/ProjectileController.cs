using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
   
     

    [HideInInspector] public float speed;
    [HideInInspector] public float weight;

    [HideInInspector] public Vector3 offset;
    [HideInInspector] public bool obeysGravity;
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

        Vector3 v3Force = speed * transform.forward;

        Debug.Log("help");

        Instantiate(rig = prefab.GetComponent<Rigidbody>(), transform.position, transform.rotation  );

        prefab.GetComponent<Rigidbody>().AddForce(v3Force);

        //Rigidbody instantiatedProjectile = Instantiate(,
        //                                                  transform.position,
        //                                                transform.rotation)
        //   as Rigidbody;

        //instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, speed));

        //if (obeysGravity)
        //{
        //  rig.useGravity = true;
        //rig.mass = weight;
        //}
        //else
        //{
        //rig.useGravity = false;

        //}

    }
}
