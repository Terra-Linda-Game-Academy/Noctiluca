using System;

namespace Input {
	[Serializable]
	public abstract class InputMiddleware<T, D> {

		[NonSerialized] public D Dispatcher;

		public abstract void TransformInput(ref T inputData);

		public abstract void Init();
		//public abstract void Release();
	}
}