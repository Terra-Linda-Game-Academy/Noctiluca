using UnityEngine;

namespace Enemies.Territorial
{
    public class TerritoryTrigger : MonoBehaviour
    {
        public TerritorialEnemyController enemyController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                enemyController.StartChase();
                Debug.Log("TRESPASS");
            }
        }
    }
}