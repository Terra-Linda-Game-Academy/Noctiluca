using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDestroy : MonoBehaviour
{

    public float aliveTime;
    private void FixedUpdate()
    {
        aliveTime += Time.deltaTime;

        if(aliveTime >= 5f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
