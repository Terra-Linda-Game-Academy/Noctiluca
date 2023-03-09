using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;
using Util;

namespace Input.Middleware.Enemy.FireSnake
{
	public class
		PerceptionMiddleware : InputMiddleware<FireSnakeInput, FireSnakeInputEvents.Dispatcher>
	{
		public float MaxViewDistance = 200.0f;
		public float MaxViewAngle = 60.0f;


		private bool trackingPlayer = false;
		public const float ObjectPermanenceTime = 5f;
		private float objectPermanenceTimer = 0f;


		public ScriptableVar<MonoBehaviour> Player;

		public override void TransformInput(ref FireSnakeInput inputData)
		{
			inputData.PlayerPos = Player.Value.transform.position;
			if (perceptron.VisionCone(Player.Value.gameObject, MaxViewDistance, MaxViewAngle / 2,
									  ~LayerMask.GetMask("Player")))
			{
				trackingPlayer = true;
				objectPermanenceTimer = 0f;
			}
			if(trackingPlayer)
            {

				if (objectPermanenceTimer >= ObjectPermanenceTime)
				{
					trackingPlayer = false;
					objectPermanenceTimer = 0f;
				}
				//Debug.Log("Chase!");

				inputData.PlayerPos = Player.Value.transform.position;
				inputData.State = FireSnakeState.Chase;

				objectPermanenceTimer += Time.fixedDeltaTime;

				
			}
			else { inputData.State = FireSnakeState.Wander; }

			//inputData.State = FireSnakeState.Wander;

			
		}

		public override void Init() { }

		public override InputMiddleware<FireSnakeInput, FireSnakeInputEvents.Dispatcher> Clone()
		{
			return new PerceptionMiddleware
			{
				trackingPlayer = trackingPlayer,
				objectPermanenceTimer = objectPermanenceTimer,
				MaxViewDistance = MaxViewDistance,
				MaxViewAngle = MaxViewAngle,
				Player = Player
			};
		}
	}
}
