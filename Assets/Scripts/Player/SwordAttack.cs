using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class SwordAttack : MonoBehaviour
    {
        public float attackRadius = 2.0f;  // radius of the attack cone
        public float attackAngle  = 45.0f; // angle of the attack cone

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Input.GetButtonDown("Fire"))
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);

                List<Collider> collidersInCone = new List<Collider>();

                foreach (Collider col in hitColliders)
                {
                    // check if the collider is within the attack cone
                    Vector3 directionToCollider = (col.transform.position - transform.position).normalized;
                    float   angleToCollider     = Vector3.Angle(transform.forward, directionToCollider);
                    if (angleToCollider <= attackAngle / 2.0f)
                    {
                        collidersInCone.Add(col);
                    }
                }

                // do something with the colliders inside the cone, like apply damage or knockback
                foreach (Collider col in collidersInCone)
                {
                    Debug.Log(col.gameObject.name + " is inside the attack cone!");
                }
            }
        }

        // draw the attack cone in the editor for debugging purposes
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Quaternion leftRayRotation  = Quaternion.AngleAxis(-attackAngle / 2.0f, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(attackAngle  / 2.0f, Vector3.up);

            Vector3 leftRayDirection  = leftRayRotation  * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;

            Gizmos.DrawLine(transform.position, transform.position + leftRayDirection  * attackRadius);
            Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * attackRadius);
        }
    }
}
