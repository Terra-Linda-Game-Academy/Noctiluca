using System.Collections.Generic;
using UnityEngine;

namespace Input {
	public class InputProvider<T> : ScriptableObject where T : new() {
		private List<InputMiddleware<T>> _middlewares;

		private T _inputStruct;

		public T GetInputData() {
			_inputStruct = new T();
			
			foreach (InputMiddleware<T> middleware in _middlewares) {
				middleware.TransformInput(ref _inputStruct);
			}

			return _inputStruct;
		}
		
		public virtual void ClearBaseObjects() {
			Debug.LogWarning("Ran base InputProvider ClearBaseObjects() method, this should never appear.");
		}
	}
}