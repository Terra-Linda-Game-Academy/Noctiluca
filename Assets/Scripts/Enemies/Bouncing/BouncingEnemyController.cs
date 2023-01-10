using UnityEngine;

namespace Enemies.Bouncing {
	public class BouncingEnemyController : MonoBehaviour {
		public float     speed    = 10;
		public float     maxSlope = 45;
		public bool      randomStartDirection;
		public Vector2   startDirection;
		public Rigidbody rb;

		private Vector2 _direction;

		void Start() {
			rb         = GetComponent<Rigidbody>();
			_direction = randomStartDirection ? Random.insideUnitCircle.normalized : startDirection;
		}

		void Update() {
			Vector3 newVelocity = new Vector3(_direction.x, 0, _direction.y) * speed;
			newVelocity.y = rb.velocity.y;
			rb.velocity   = newVelocity;
		}

		private void OnCollisionEnter(Collision collision) {
			Vector3 norm = collision.GetContact(0).normal;
			if (norm.y <= Mathf.Cos(Mathf.Deg2Rad * maxSlope)) {
				_direction = Vector2.Reflect(_direction, new Vector2(norm.x, norm.z).normalized);
				Debug.Log(_direction);
				_direction.Normalize();
			}
		}
	}
}