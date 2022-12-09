using UnityEngine;

namespace Enemies.Walking {
	public class WalkingEnemyController : MonoBehaviour {
		public  Transform player;
		private Vector3   _playerPos;

		public bool  isWalking;
		public float speed;
		public float minDistance;

		public float maxViewRange     = 20.0f;
		public float maxViewAngle     = 60.0f;

		public Transform eyes;

		private float _distToPlayer;
		private bool  _canSeePlayer;

		private void FixedUpdate() {
			_playerPos   = player.position;
			_playerPos.y = transform.position.y;
			
			Vector3 toPlayer = _playerPos - transform.position;
			_distToPlayer = toPlayer.magnitude;

			CheckVision(toPlayer);

			WalkToPlayer();
		}

		private void OnDrawGizmos() {
			//draw player pos sphere
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(_playerPos, 0.2f);

			if (!_canSeePlayer) {
				Vector3 leftSightLineEnd =
					eyes.position + Quaternion.Euler(0, -maxViewAngle / 2, 0) * eyes.forward * maxViewRange;
				Vector3 rightSightLineEnd =
					eyes.position + Quaternion.Euler(0, maxViewAngle / 2, 0) * eyes.forward * maxViewRange;

				Debug.DrawLine(eyes.position, leftSightLineEnd, Color.yellow);
				Debug.DrawLine(eyes.position, rightSightLineEnd, Color.yellow);
			} else { Debug.DrawLine(eyes.position, _playerPos, Color.red); }
		}

		private void CheckVision(Vector3 toPlayer) {
			toPlayer.Normalize();

			if (Vector3.Angle(eyes.forward, toPlayer) <= maxViewAngle / 2) {
				_canSeePlayer = !Physics.Raycast(eyes.position, toPlayer, _distToPlayer, ~LayerMask.GetMask("Player"));
			}
		}

		private void WalkToPlayer() {
			if (!isWalking || !_canSeePlayer) return;

			transform.LookAt(_playerPos);

			if (_distToPlayer > minDistance) {
				Vector3 walkVec = transform.forward * speed;
				walkVec.y          =  0;
				transform.position += walkVec;
			}
		}
	}
}