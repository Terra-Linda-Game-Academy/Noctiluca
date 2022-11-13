using System;

namespace Input.Events {
	public abstract class Dispatcher<T> {
		private readonly Func<T> _inputFunc;

		protected T GetInput() => _inputFunc.Invoke();

		protected Dispatcher(Func<T> inputFunc) {
			_inputFunc = inputFunc;
		}
	}
}