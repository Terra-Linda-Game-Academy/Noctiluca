using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{

    public IceSheetController targetIceSheet = null;

 

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.other.CompareTag("Water") && targetIceSheet != null)
        {
            targetIceSheet.AddPointToShape(transform.position);
            Destroy(gameObject);
        }
    }
}
