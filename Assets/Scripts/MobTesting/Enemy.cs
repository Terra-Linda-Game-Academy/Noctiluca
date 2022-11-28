using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyStateMachine enemyStateMachine;

    void Start()
    {
        enemyStateMachine = new EnemyStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
