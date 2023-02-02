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

        prefab.GetComponent<Rigidbody>().useGravity = true;

        prefab.GetComponent<Rigidbody>().AddForce(v3Force);

        Debug.Log("boobies");

    }
}
