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
			foreach (InputMiddleware<T, D> middleware in _middlewares) { middleware.TransformInput(ref input); }
			return input;
		}

		private void OnValidate() => ClearBaseObjects();

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

		// IList implementation methods

		/*public IEnumerator<InputMiddleware<T, D>> GetEnumerator() {
			foreach (object obj in _middlewares) 
				yield return (InputMiddleware<T, D>) obj;
		}
		
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		public void Add(InputMiddleware<T, D> item) {
			if (item is null) {
				Debug.LogWarning("Attempted to add null middleware to InputProvider, blocked.");
				return;
			}
			_middlewares.Add(item);
		}
		public void Clear() => _middlewares.Clear();
		public bool Contains(InputMiddleware<T, D> item) => _middlewares.Contains(item);
		public void CopyTo(InputMiddleware<T, D>[] array, int arrayIndex) {
			for (int i = arrayIndex; i < array.Length; i++) {
				array[i] = (InputMiddleware<T, D>) _middlewares[i];
			}
		}

		public bool Remove(InputMiddleware<T, D> item) {
			bool itemPresent = _middlewares.Remove(item);
			if (itemPresent) item?.Release();
			return itemPresent;
		}
		
		public int Count => _middlewares.Count;
		public bool IsReadOnly => false; //todo: is this right?
		public int IndexOf(InputMiddleware<T, D> item) => _middlewares.IndexOf(item);

		public void Insert(int index, InputMiddleware<T, D> item) {
			if (item is null) {
				Debug.LogWarning("Attempted to add null middleware to InputProvider, blocked.");
				return;
			}
			_middlewares[index] = item;
			item.Init();
		}

		public void RemoveAt(int index) {
			((InputMiddleware<T, D>) _middlewares[index]).Release();
			_middlewares.RemoveAt(index);
		}

		public InputMiddleware<T, D> this[int index] {
			get => (InputMiddleware<T, D>) _middlewares[index];
			set {
				((InputMiddleware<T, D>) _middlewares[index]).Release();
				_middlewares[index] = value;
				value.Dispatcher = _dispatcher;
				value.Init();
			}
		}*/
	}
}