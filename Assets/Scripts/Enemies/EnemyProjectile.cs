using Player;
using UnityEngine;

namespace Enemies {
	[RequireComponent(typeof(Rigidbody))]
	public class EnemyProjectile : MonoBehaviour {
		public float lifetime;
		public float force;
		Rigidbody    _rigidbody;

		private void Start() {
			Destroy(gameObject, lifetime);

			_rigidbody = GetComponent<Rigidbody>();
			_rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
		}

		private void OnCollisionEnter(Collision other) {
			if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

			other.gameObject.GetComponent<PlayerHealthController>().Damage(1);
			Destroy(gameObject);
		}
	}
}