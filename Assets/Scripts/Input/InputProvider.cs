using System.Collections.Generic;
using UnityEngine;

namespace Input {
	public class InputProvider<T, E> : ScriptableObject where T : new() where E : new() {
		// ReSharper disable once CollectionNeverUpdated.Local
		// ReSharper disable once FieldCanBeMadeReadOnly.Local
		[SerializeReference] private List<object> _middlewares = new();

		public E events;

		private T _inputStruct;

		public InputProvider() { events = new E(); }

		public T GetInputData() {
			_inputStruct = new T();

			foreach (InputMiddleware<T, E> middleware in _middlewares) {
				middleware.TransformInput(ref _inputStruct, ref events);
			}

			return _inputStruct;
		}

		private void OnValidate() { ClearBaseObjects(); }

		private void ClearBaseObjects() {
			int nullObjs = 0;

			foreach (object middleware in _middlewares) {
				if (middleware == null) {
					nullObjs++;
					Debug.LogWarning("Please add middlewares with the dedicated \"Add Middleware\" button.");
				}
			}

			for (int i = 0; i < nullObjs; i++) _middlewares.Remove(null);
		}
	}
}