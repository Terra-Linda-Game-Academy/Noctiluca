using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class EnemyPool : EnemySpawner
    {
        public EnemyDefinition enemyDefinition;

        private int totalWeight;

        public int spawnWeight;

        private void CalculateTotalWeight()
        {
            totalWeight = 0;
            foreach (EnemyPool pool in enemyPool)
            {
                totalWeight += pool.spawnWeight;
            }
        }
        public EnemyDefinition GetRandomEnemyDefinition()
        {
            int randomValue = Random.Range(0, totalWeight);
            int weightSum = 0;

            foreach (EnemyPool pool in enemyPool)
            {
                weightSum += pool.spawnWeight;
                if (randomValue < weightSum)
                {
                    return pool.enemyDefinition;
                }
            }

            return null;
        }
    }
}