using System;
using UnityEngine.UIElements;

namespace Input {
	[Serializable]
	public class InputMiddleware<T> {
		public virtual void TransformInput(ref T inputData) { }

		public virtual VisualElement Body() { return new VisualElement(); }
	}
}