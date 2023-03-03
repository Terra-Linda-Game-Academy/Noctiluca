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
		public float WanderRadius = 10.0f;
		float waitTime;

		public override void TransformInput(ref FireSnakeInput inputData)
		{
			switch(inputData.State)
            {
				case FireSnakeState.Chase:
					inputData.TargetDestination = inputData.PlayerPos;
					break;
				case FireSnakeState.Wander:
					inputData.TargetDestination = RandomNavSphere(perceptron.transform.position, WanderRadius, -1);
					break;


			}
		}

		public override void Init() { }

		public override InputMiddleware<FireSnakeInput, FireSnakeInputEvents.Dispatcher> Clone()
		{
			return new MovementMiddleware {
				WanderRadius = WanderRadius,
				waitTime = waitTime
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