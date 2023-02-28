using System;
using AI;

namespace Input.Events {
	public abstract class EventDispatcher<T> {
		private readonly Func<T> _inputFunc;

		protected T GetInput() => _inputFunc.Invoke();

		protected EventDispatcher(Func<T> inputFunc) {
			_inputFunc = inputFunc;
		}
	}
}