using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class EnemySpawner : SimpleTile
    {
        [SerializeField]
        private GameObject enemy;

        [SerializeField]
        private EnemyPool enemyPool;

        [SerializeField]
        private int singleEnemySpawnCount;

        [SerializeField]
        private Vector3 centerPosition;

        [SerializeField]
        private int EnemiesToScatter;

        [SerializeField]
        private float averageDistanceBetweenEnemies;

        [SerializeField]
        private float roomHeight;

        


        //can i get uhhhhhhhhhh weight calc?
        
        //Define the enemy, define its position, and then initialize the enemy at the spawn position parenting the controller.
        public void SpawnSingleEnemy()
        {
            EnemyDefinition enemyDefinition = enemyPool.GetRandomEnemyDefinition();
            Vector3 spawnPosition = GetValidEnemyPosition();
            Init(enemy);
        }

        // spawn many enemies = spawn one enemy a lot
        public void ScatterEnemies()
        {
            for (int i = 0; i < EnemiesToScatter; i++)
            {
                SpawnSingleEnemy();
            }
        }

        private Vector3 GetValidEnemyPosition()
        {
            Vector3 spawnPosition = centerPosition + new Vector3(Random.Range(-averageDistanceBetweenEnemies, averageDistanceBetweenEnemies), 0, Random.Range(-averageDistanceBetweenEnemies, averageDistanceBetweenEnemies));
            RaycastHit hit;

            if (Physics.Raycast(new Vector3(spawnPosition.x, roomHeight, spawnPosition.z), Vector3.down, out hit))
            {
                spawnPosition.y = hit.point.y;
            }
            return spawnPosition;
        }
    }
}