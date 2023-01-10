using UnityEngine;
using UnityEngine.AI;

namespace Enemies.ChargeMob {
	public class BasicMob : MonoBehaviour {
		public float wanderRadius;
		public float wanderTimer;

		protected NavMeshAgent Agent;
		protected float        Timer;

		protected bool IsWandering = true;


		void OnEnable() {
			Agent = GetComponent<NavMeshAgent>();
			Timer = wanderTimer;
		}

		protected void Awake() { updateFunctions += WanderAround; }

		protected delegate void UpdateFunctions();

		protected UpdateFunctions updateFunctions;

		void Update() { updateFunctions(); }

		void WanderAround() {
			if (IsWandering) {
				Timer += Time.deltaTime;

				if (Timer >= wanderTimer) {
					Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
					Agent.SetDestination(newPos);
					Timer = 0;
				}
			}
		}


		public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
			Vector3 randDirection = Random.insideUnitSphere * dist;

			randDirection += origin;

			NavMeshHit navHit;

			NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

			return navHit.position;
		}
	}
}