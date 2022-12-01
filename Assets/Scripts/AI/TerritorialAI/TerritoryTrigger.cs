using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryTrigger : MonoBehaviour
{
    public TerritorialAIController aiController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            aiController.StartChase();
            Debug.Log("TRESPASS");
        }
    }
}
