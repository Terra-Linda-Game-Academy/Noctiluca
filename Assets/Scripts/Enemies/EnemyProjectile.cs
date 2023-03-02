using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyProjectile : MonoBehaviour
    {
        public float lifetime;
        public float force;
        Rigidbody _rigidbody;

        private void Awake()
        {
            Destroy(gameObject, lifetime);

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.AddForce(transform.forward * force);
        }
    }
}