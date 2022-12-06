using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Territorial {
	public class TerritorialEnemyController : MonoBehaviour {
		public NavMeshAgent navMeshAgent;
		public float        startWaitTime = 4;
		public float        timeToRotate  = 2;
		public float        speedWalk     = 6;
		public float        speedRun      = 9;

		public float     viewRadius = 15;
		public float     viewAngle  = 90;
		public LayerMask playerMask;
		public LayerMask obstacleMask;
		public float     meshResolution = 1f;
		public int       edgeIterations = 4;
		public float     edgeDistance   = 0.5f;

		public Transform[] waypoints;
		int                _currentWaypointIndex;

		Vector3 _playerLastPosition = Vector3.zero;
		Vector3 _playerPosition;

		private float _waitTime;
		private float _timeToRotate;
		private bool  _playerInRange;
		private bool  _playerNear;
		private bool  _caughtPlayer;
		private bool  _isPatrol;

		void Start() {
			_playerPosition = Vector3.zero;
			_isPatrol       = true;
			_caughtPlayer   = false;
			_playerInRange  = false;
			_waitTime       = startWaitTime;
			_timeToRotate   = timeToRotate;

			_currentWaypointIndex = 0;
			navMeshAgent           = GetComponent<NavMeshAgent>();

			navMeshAgent.isStopped = false;
			navMeshAgent.speed     = speedWalk;
			navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
		}

		// Update is called once per frame
		void Update() {
			EnvironmentView();

			if (!_isPatrol) { Chasing(); } else { Patrolling(); }
		}

		private void Chasing() {
			_playerNear         = false;
			_playerLastPosition = Vector3.zero;

			if (!_caughtPlayer) {
				Move(speedRun);
				navMeshAgent.SetDestination(_playerPosition);
			}

			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
				_isPatrol   = true;
				_playerNear = false;
				Move(speedWalk);
				_timeToRotate = timeToRotate;
				_waitTime     = startWaitTime;
				navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
			}
		}

		private void Patrolling() {
			if (_playerNear) {
				if (_timeToRotate <= 0) {
					Move(speedWalk);
					LookingPlayer(_playerLastPosition);
				} else {
					Stop();
					_timeToRotate -= Time.deltaTime;
				}
			} else {
				_playerNear         = false;
				_playerLastPosition = Vector3.zero;
				navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
				if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
					if (_waitTime <= 0) {
						NextPoint();
						Move(speedWalk);
						_waitTime = startWaitTime;
					} else {
						Stop();
						_waitTime -= Time.deltaTime;
					}
				}
			}
		}

		void Move(float speed) {
			navMeshAgent.isStopped = false;
			navMeshAgent.speed     = speed;
		}

		public void StartChase() { _isPatrol = false; }

		public void NextPoint() {
			_currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
			navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
		}

		void Stop() {
			navMeshAgent.isStopped = true;
			navMeshAgent.speed     = 0;
		}

		void CaughtPlayer() { _caughtPlayer = true; }

		void LookingPlayer(Vector3 player) {
			navMeshAgent.SetDestination(player);
			if (Vector3.Distance(transform.position, player) <= 0.3) {
				if (_waitTime <= 0) {
					_playerNear = false;
					Move(speedWalk);
					navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
					_waitTime     = startWaitTime;
					_timeToRotate = timeToRotate;
				} else {
					Stop();
					_waitTime -= Time.deltaTime;
				}
			}
		}

		void EnvironmentView() {
			Collider[]
				playerInRange =
					Physics.OverlapSphere(transform.position, viewRadius,
					                      playerMask);

			foreach (Collider playerCollider in playerInRange) {
				Transform player      = playerCollider.transform;
				Vector3   dirToPlayer = (player.position - transform.position).normalized;
				if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2) {
					float dstToPlayer =
						Vector3.Distance(transform.position, player.position);
					if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask)) {
						_playerInRange =
							true;
						_isPatrol = false;
					} else { _playerInRange = false; }
				}

				if (Vector3.Distance(transform.position, player.position) > viewRadius) { _playerInRange = false; }

				if (_playerInRange) {
					_playerPosition =
						player.transform
						      .position;
				}
			}
		}
	}
}