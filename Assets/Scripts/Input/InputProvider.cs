using System.Collections.Generic;
using UnityEngine;

namespace Input {
	public class InputProvider<T> : ScriptableObject where T : new() {
		[SerializeReference] private List<object> _middlewares = new();

		private T _inputStruct;

		public T GetInputData() {
			_inputStruct = new T();
			
			foreach (InputMiddleware<T> middleware in _middlewares) {
				middleware.TransformInput(ref _inputStruct);
			}

			return _inputStruct;
		}

		private void OnValidate() { ClearBaseObjects(); }

		private void ClearBaseObjects() {
			List<object> toBeDeleted = new List<object>();

			foreach (object middleware in _middlewares) {
				if (middleware == null) {
					toBeDeleted.Add(middleware);
					Debug.LogWarning("Please add middlewares with the dedicated \"Add Middleware\" button.");
				}
			}

			foreach (object delete in toBeDeleted) {
				_middlewares.Remove(delete);
			}
		}
	}
}