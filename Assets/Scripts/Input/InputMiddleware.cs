using System;

namespace Input {
	[Serializable]
	public abstract class InputMiddleware<T, D> {
		public D Dispatcher { protected get; set; }

		public abstract void TransformInput(ref T inputData);

		public abstract void Init();
	}
}