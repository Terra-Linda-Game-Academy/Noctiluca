using System;

namespace Input {
	[Serializable]
	public class InputMiddleware<T> {
		public virtual void TransformInput(ref T inputData) { }
	}
}