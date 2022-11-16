using System;

namespace Input.Events {
	public interface IInputEvents<in T, out D> {
		public D GetDispatcher(Func<T> inputFunc);
	}
}