using System.Collections.Generic;
using UnityEngine;

namespace Input {
	public class InputProvider<T> : ScriptableObject {
		public List<InputMiddleware<T>> middlewares;

		public void DebugPrint() {
			Debug.Log("middlewares:");
			
			foreach (InputMiddleware<T> middleware in middlewares) Debug.Log(middleware);
		}
	}
}