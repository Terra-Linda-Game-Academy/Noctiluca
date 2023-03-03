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
		public float MaxViewDistance = 20.0f;
		public float MaxViewAngle = 60.0f;


		public ScriptableVar<MonoBehaviour> Player;

		public override void TransformInput(ref FireSnakeInput inputData)
		{
			inputData.PlayerPos = Player.Value.transform.position;
			if (perceptron.VisionCone(Player.Value.gameObject, MaxViewDistance, MaxViewAngle / 2,
									  ~LayerMask.GetMask("Player")))
			{
				inputData.PlayerPos = Player.Value.transform.position;
				inputData.State = FireSnakeState.Chase;
			}
			else { inputData.State = FireSnakeState.Wander; }
		}

		public override void Init() { }

		public override InputMiddleware<FireSnakeInput, FireSnakeInputEvents.Dispatcher> Clone()
		{
			return new PerceptionMiddleware
			{
				MaxViewDistance = MaxViewDistance,
				MaxViewAngle = MaxViewAngle,
				Player = Player
			};
		}
	}
}
