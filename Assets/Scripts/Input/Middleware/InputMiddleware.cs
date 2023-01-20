using System;
using Input.Events;

namespace Input.Middleware {
	[Serializable]
	public abstract class InputMiddleware<T, D> where D : EventDispatcher<T> {
		public D Dispatcher { protected get; set; }

		public abstract void TransformInput(ref T inputData);

		public abstract void Init();
	}
}