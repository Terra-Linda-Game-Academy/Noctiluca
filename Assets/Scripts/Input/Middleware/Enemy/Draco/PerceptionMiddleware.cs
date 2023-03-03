using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;
using Util;

namespace Input.Middleware.Enemy.Draco {
	public class PerceptionMiddleware : InputMiddleware<DracoInput, DracoInputEvents.Dispatcher> {
		public float MaxViewDistance = 20.0f;
		public float MaxViewAngle    = 100.0f;

		public ScriptableVar<MonoBehaviour> Player;

		public override void TransformInput(ref DracoInput inputData) {
			if (perceptron.VisionCone(Player.Value.gameObject, MaxViewDistance, MaxViewAngle,
			                          ~LayerMask.GetMask("Player"))) {
				inputData.PlayerPos = Player.Value.transform.position;
				inputData.State     = DracoStates.Aggro;
			} else { inputData.State = DracoStates.Wander; }
		}

		public override void Init() { }

		public override InputMiddleware<DracoInput, DracoInputEvents.Dispatcher> Clone() {
			return new PerceptionMiddleware {
				                                MaxViewDistance = MaxViewDistance,
				                                MaxViewAngle    = MaxViewAngle,
				                                Player          = Player
			                                };
		}
	}
}