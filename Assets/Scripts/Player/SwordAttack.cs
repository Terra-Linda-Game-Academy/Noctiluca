using System.Collections.Generic;
using UnityEngine;

namespace Player {
	public class SwordAttack : MonoBehaviour {
		public float attackRadius = 2.0f;  // radius of the attack cone
		public float attackAngle  = 45.0f; // angle of the attack cone

		[HideInInspector] public Vector3 attackDir;

		private const int MaxHits = 32;

		public void Attack() {
			Collider[] hitColliders = new Collider[MaxHits];
			int        numColliders = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, hitColliders);

			List<Collider> collidersInCone = new List<Collider>();

			for (int i = 0; i < numColliders; i++) {
				Collider col = hitColliders[i];
				
				if (col.gameObject == gameObject || col.gameObject.layer == LayerMask.NameToLayer("Room")) continue;

				// check if the collider is within the attack cone
				Vector3 directionToCollider = (col.transform.position - transform.position).normalized;
				float   angleToCollider     = Vector3.Angle(attackDir, directionToCollider);
				if (angleToCollider <= attackAngle / 2.0f) { collidersInCone.Add(col); }
			}

			// do something with the colliders inside the cone, like apply damage or knockback
			foreach (Collider col in collidersInCone) {
				Debug.Log(col.gameObject.name + " is inside the attack cone!");
			}
		}

		// draw the attack cone in the editor for debugging purposes
		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;

			Quaternion leftRayRotation  = Quaternion.AngleAxis(-attackAngle / 2.0f, Vector3.up);
			Quaternion rightRayRotation = Quaternion.AngleAxis(attackAngle  / 2.0f, Vector3.up);

			Vector3 leftRayDirection  = leftRayRotation  * attackDir;
			Vector3 rightRayDirection = rightRayRotation * attackDir;

			Gizmos.DrawLine(transform.position, transform.position + leftRayDirection  * attackRadius);
			Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * attackRadius);
		}
	}
}