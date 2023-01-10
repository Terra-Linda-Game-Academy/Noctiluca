using Input.Data.Enemy;
using Input.Events.Enemy;

namespace Input.Middleware.Enemy {
	public class WalkingEnemyInputMiddleware : InputMiddleware<WalkingEnemyInput, WalkingEnemyInputEvents.Dispatcher> {
		public override void TransformInput(ref WalkingEnemyInput inputData) {
			
		}

		public override void Init() {
			
		}
	}
}