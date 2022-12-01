using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargeMob : BasicMob
{
    public float agroRadius = 20f;
    public GameObject player;

    Vector3 startingLocation;
    Vector3 chargeLocation;
    bool isAgro = false;
    float maxDistanceRun = 0f;

    float chargeCooldown = 0f;
    float chargeTime = 0f;


    void Awake()
    {
        base.Awake();
        updateFunctions += detectPlayer;
        updateFunctions += chargeUpdate;
    }

    void detectPlayer()
    {
        chargeCooldown -= Time.deltaTime;
        if (chargeCooldown > 0f || isAgro) return;
        if(Vector3.Distance(transform.position, player.transform.position) < agroRadius)
        {
            RaycastHit hit;
            Vector3 rayDirection = player.transform.position- transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(rayDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,Time.deltaTime*10);
            Debug.DrawRay(transform.position, rayDirection, Color.blue);
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.transform.gameObject == player)
                {
                    isWandering = false;
                    chargeCooldown = 30f;
                    startingLocation = transform.position;
                    maxDistanceRun = Vector3.Distance(transform.position, player.transform.position) + 2f;
                    chargeLocation = player.transform.position;

                    isAgro = true;
                    
                    
                    StartCoroutine(chargeAtLocaiton());
                }
                
            }

            
        } else
        {
            isWandering = true;
        }
        
    }

    IEnumerator chargeAtLocaiton()
    {
        // chargin period (stands still while getting ready to charge at you)
        yield return new WaitForSeconds(0.3f);
        agent.SetDestination(chargeLocation);
        agent.speed = 40f;
        agent.angularSpeed = 5;
    }

    void chargeUpdate()
    {

        if (isAgro)
        {
            chargeTime += Time.deltaTime;
            if(Vector3.Distance(transform.position, startingLocation) > maxDistanceRun || Vector3.Distance(transform.position, chargeLocation) < 1f || chargeTime > 10f) {
                chargeTime = 0f;
                isAgro = false;
                isWandering = true;
                agent.speed = 4.67f;
                agent.angularSpeed = 120f;
            }
        }
        
        

    }

}

