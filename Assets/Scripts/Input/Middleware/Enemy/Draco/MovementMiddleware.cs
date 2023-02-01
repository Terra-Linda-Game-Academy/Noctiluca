using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;
using Util;

namespace Input.Middleware.Enemy.Draco {
	public class MovementMiddleware : InputMiddleware<DracoInput, DracoInputEvents.Dispatcher> {
		public ScriptableVar<Vector3> Center;
		public ScriptableVar<Vector3> Radii;

		public float MinWaypointDist  = 0.5f;
		public float WaypointCooldown = 1.0f;

		public float MaxAttackRange = 5.0f;
		public float AttackCooldown = 1.0f;

		private Vector3 _waypoint;
		private float   _waypointCountUp;

		private float _attackCountDown;

		public override void TransformInput(ref DracoInput inputData) {
			Vector3 position = perceptron.transform.position;

			switch (inputData.State) {
				case DracoStates.Wander:
					//wandering state, go towards some random point in the range, or if not in range go back to range

					if (perceptron.InRectRangeOfPoint(perceptron.gameObject, Center.Value, Radii.Value)) {
						//in territory range

						if ((_waypoint - position).magnitude <= MinWaypointDist) {
							//at waypoint, check if cooldown is up

							if (_waypointCountUp > WaypointCooldown) {
								//if the cooldown is up

								_waypointCountUp = 0.0f;

								float x = Random.Range(-Radii.Value.x, Radii.Value.x);
								float y = Random.Range(-Radii.Value.y, Radii.Value.y);
								float z = Random.Range(-Radii.Value.z, Radii.Value.z);

								_waypoint = Center.Value + new Vector3(x, y, z);
							} else {
								//wait for cooldown
								_waypointCountUp += 1f / 50f;
							}
						}
					} else {
						//not in range, move back towards range center

						_waypoint = Center.Value;
					}

					float distToWaypoint = (_waypoint - position).magnitude;
					if (distToWaypoint >= MinWaypointDist) {
						inputData.Movement = (_waypoint - position).normalized;
						inputData.LookDir  = Quaternion.LookRotation(inputData.Movement);
					} else {
						inputData.Movement = Vector3.zero;
						inputData.LookDir  = Quaternion.identity;
					}

					break;
				case DracoStates.Aggro:
					//aggro state, if not in attack range move towards player, otherwise stop & shoot

					Vector3 toPlayer = inputData.PlayerPos - position;

					if (toPlayer.magnitude <= MaxAttackRange) {
						inputData.Movement = Vector3.zero;
						inputData.LookDir  = Quaternion.LookRotation(toPlayer);

						if (_attackCountDown <= 0f) {
							_attackCountDown = AttackCooldown;
							Dispatcher.Shoot();
						}
					} else {
						inputData.Movement = toPlayer.normalized;
						inputData.LookDir  = Quaternion.LookRotation(inputData.Movement);
					}

					if (_attackCountDown > 0) _attackCountDown -= 1f / 50f;

					break;
			}
		}

		public override void Init() { }

		public override InputMiddleware<DracoInput, DracoInputEvents.Dispatcher> Clone() {
			return new MovementMiddleware {
				                              Center          = Center,
				                              Radii           = Radii,
				                              MinWaypointDist = MinWaypointDist,
				                              MaxAttackRange  = MaxAttackRange
			                              };
		}
	}
}