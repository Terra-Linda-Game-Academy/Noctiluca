using System;
using System.Collections.Generic;
using Input.Events;
using Input.Middleware;
using UnityEngine;

namespace Input {
	public class InputProvider<T, E, D> : ScriptableObject//, IList<InputMiddleware<T, D>>
	where T : struct
	where E : class, IInputEvents<T, D>, new() 
	where D : EventDispatcher<T> {
		[SerializeReference] private List<object> _middlewares = new();
		
		public E Events { get; private set; }
		[NonSerialized] private D _dispatcher;
		[NonSerialized] private bool _initted;

		public T GetInput() {
			if (!_initted) throw new NotInittedException();

			var input = new T();
			foreach (InputMiddleware<T, D> middleware in _middlewares) {
				middleware.TransformInput(ref input);
			}
			return input;
		}

		private void OnValidate() => ClearBaseObjects();

		private void ClearBaseObjects() { //todo: remove when listviewer is done
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
			Events      = new E();
			_dispatcher = Events.GetDispatcher(GetInput);

			foreach (InputMiddleware<T, D> middleware in _middlewares) {
				middleware.Dispatcher = _dispatcher;
				middleware.Init();
			}

			_initted = true;
		}

		private class NotInittedException : Exception {
			public override string Message => "InputProvider was not initialized before using! Run RequireInit() first.";
		}

		public static void Create(params InputMiddleware<T, D>[] middlewares) {
			var newObj = CreateInstance<InputProvider<T, E, D>>();
			newObj._middlewares.AddRange(middlewares);
			newObj.RequireInit();
		}
	}
}