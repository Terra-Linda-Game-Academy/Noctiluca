using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBobController : MonoBehaviour
{
    //public AnimationCurve myCurve;
    public float strength = 5f;
    public float timeMultiplier = 0.1f;

    //Vector3 normalPostiion;

    private void Awake()
    {
        //normalPostiion = transform.position;
    }

    private void Update()
    {   
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * timeMultiplier) * strength * Time.deltaTime, transform.position.z);
    }
}
