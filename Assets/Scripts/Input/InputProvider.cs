using UnityEngine;

namespace Input {
	public class InputProvider : ScriptableObject {
		public virtual void ClearBaseObjects() {
			Debug.LogWarning("Ran base InputProvider ClearBaseObjects() method, this should never appear.");
		}
	}
}