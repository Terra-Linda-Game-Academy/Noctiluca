using UnityEngine;

namespace AI.Old {
	public abstract class Action : ScriptableObject {
		public abstract void Act(StateController controller);
	}
}