using System;

namespace Input.Middleware {
	[Serializable]
	public abstract class InputMiddleware<T, D> {
		public D Dispatcher { protected get; set; }

		public abstract T TransformInput(T inputData);

		public abstract void Init();
	}
}