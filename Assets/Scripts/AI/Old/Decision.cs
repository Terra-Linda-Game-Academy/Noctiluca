using UnityEngine;

namespace AI.Old {
	public abstract class Decision : ScriptableObject {
		public abstract bool Decide(StateController controller);
	}
}