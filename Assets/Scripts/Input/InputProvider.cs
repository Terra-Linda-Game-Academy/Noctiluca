using UnityEngine;

namespace Input {
	public class InputProvider : ScriptableObject {
		//public List<InputMiddleware<T>> middlewares;

		/*public void DebugPrint() {
			Debug.Log("middlewares:");
			
			foreach (InputMiddleware<T> middleware in middlewares) Debug.Log(middleware);
		}*/

		public virtual void ClearBaseObjects() {
			Debug.LogWarning("Ran base InputProvider ClearBaseObjects() method, this should never appear.");
		}
	}
}