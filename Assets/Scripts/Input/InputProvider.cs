using System;
using System.Collections.Generic;
using Input.Events;
using UnityEngine;

namespace Input {
	public class InputProvider<T, E, D> : ScriptableObject
		where T : new() where E : IInputEvents<T, D>, new() {
		// ReSharper disable once CollectionNeverUpdated.Local
		// ReSharper disable once FieldCanBeMadeReadOnly.Local
		[SerializeReference] private List<object> _middlewares = new();

		[NonSerialized] private T _inputStruct;
		public  E Events { get; private set; }
		[NonSerialized] private D _dispatcher;

		[NonSerialized] private bool _initted;

		public T GetInputData() {
			if (!_initted) throw new NotInittedException();

			_inputStruct = new T();

			foreach (InputMiddleware<T, D> middleware in _middlewares) { middleware.TransformInput(ref _inputStruct); }

			return _inputStruct;
		}

		private void OnValidate() { ClearBaseObjects(); }

		private void ClearBaseObjects() {
			int nullObjs = 0;

			foreach (object middleware in _middlewares) {
				if (middleware is null) {
					nullObjs++;
					Debug.LogWarning("Please add middlewares with the dedicated \"Add Middleware\" button.");
				}
			}

			for (int i = 0; i < nullObjs; i++) _middlewares.Remove(null);
		}

		public void RequireInit() {
			if (_initted) return;
			Debug.Log("initiated");

			Events      = new E();
			_dispatcher = Events.GetDispatcher(GetInputData);

			foreach (InputMiddleware<T, D> middleware in _middlewares) {
				middleware.Dispatcher = _dispatcher;
				middleware.Init();
			}

			_initted = true;
		}

		private class NotInittedException : Exception {
			public override string Message => "InputProvider was not initialized before using! Run RequireInit() first.";
		}
	}
}