using UnityEngine;

namespace Enemies {
	public class WalkingEnemyController : MonoBehaviour {
		public Vector3 playerPos;

		public bool  isWalking;
		public float speed;
		public float minDistance;

		private float _distToPlayer;

		private void Update() {
			Vector3 toPlayer = playerPos - transform.position;
			_distToPlayer = toPlayer.magnitude;

			Vector3 heightNormalizedPlayerPos = playerPos;
			heightNormalizedPlayerPos.y = transform.position.y;
			
			transform.LookAt(heightNormalizedPlayerPos);

			if (_distToPlayer > minDistance && isWalking) {
				Vector3 walkVec = transform.forward * speed;
				walkVec.y          =  0;
				transform.position += walkVec;
			}
		}

		private void OnDrawGizmos() {
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(playerPos, 0.2f);
		}
	}
}