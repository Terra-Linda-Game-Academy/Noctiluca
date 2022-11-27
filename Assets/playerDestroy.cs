using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDestroy : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
