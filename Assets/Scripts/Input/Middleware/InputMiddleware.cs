using System;
using AI;
using Input.Events;
using UnityEngine;

namespace Input.Middleware {
	[Serializable]
	public abstract class InputMiddleware<T, D> where D : EventDispatcher<T> {
		public D Dispatcher { protected get; set; }

		[HideInInspector] public Perceptron perceptron;

		public abstract void TransformInput(ref T inputData);

		public abstract void Init();

		// Allows middlewares to carry certain data when cloning
		public abstract InputMiddleware<T, D> Clone();
	}
}