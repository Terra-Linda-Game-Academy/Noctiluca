using System;
using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Input.Middleware.Enemy.FireSnake
{
	public class
		MovementMiddleware : InputMiddleware<FireSnakeInput, FireSnakeInputEvents.Dispatcher>
	{
		public const float WanderRadius = 100.0f;
		public const float WaitTime = 5f;
		private float WaitTimer = Mathf.Infinity;

		public const float regenerationProximity = 10f;

		public Vector3 lastDestination;

		public override void TransformInput(ref FireSnakeInput inputData)
		{
			//Debug.Log("Player Pos: " + inputData.PlayerPos);
			switch(inputData.State)
            {
				case FireSnakeState.Chase:
					inputData.TargetDestination = inputData.PlayerPos;
					lastDestination = inputData.TargetDestination;
					break;
				case FireSnakeState.Wander:
					//Debug.Log("Wait Time: " + WaitTime + ", Wait Timer: " + WaitTimer);
					//Debug.Log("Wander: " + Vector3.Distance(perceptron.transform.position, lastDestination));
					if (WaitTimer >= WaitTime || Vector3.Distance(perceptron.transform.position, lastDestination) < regenerationProximity)
                    {
						WaitTimer = 0f;
						//Debug.Log("Generate!");
						inputData.TargetDestination = RandomNavSphere(perceptron.transform.position, WanderRadius, -1);
						//GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = inputData.TargetDestination;
						lastDestination = inputData.TargetDestination;
					}
					
					break;


			}

			inputData.TargetDestination = lastDestination;

			WaitTimer += Time.fixedDeltaTime;

			//Debug.Log("Target Destination: " + inputData.TargetDestination);
		}

		public override void Init() { }

		public override InputMiddleware<FireSnakeInput, FireSnakeInputEvents.Dispatcher> Clone()
		{
			return new MovementMiddleware {
				WaitTimer = WaitTimer,
				lastDestination = lastDestination,
			};
		}

		public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
		{
			Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;

			randDirection += origin;

			NavMeshHit navHit;

			NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

			return navHit.position;
		}
	}
}