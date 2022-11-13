using System;
using UnityEngine;

namespace Input.Events {
	public struct PlayerInputEvents : IInputEvents<PlayerInputData, PlayerInputEvents.Dispatcher> {
		public event Action OnInteract;

		private void InvokeInteract() { OnInteract?.Invoke(); }

		public Dispatcher GetDispatcher(Func<PlayerInputData> inputFunc) {
			Dispatcher dispatcher = new Dispatcher(inputFunc, InvokeInteract);
			return dispatcher;
		}

		public class Dispatcher : Dispatcher<PlayerInputData> {
			public Dispatcher(Func<PlayerInputData> inputFunc, Action invokeInteract) : base(inputFunc) {
				invokeInteract  += () => { Debug.Log("bruh"); };
				_invokeInteract =  invokeInteract;
			}

			private readonly Action _invokeInteract;

			public void Interact() {
				Debug.Log("tried to interact");
				//demo event blocking, cant interact while moving
				PlayerInputData inputData = GetInput();
				//if (inputData.movement.magnitude < .2) _invokeInteract?.Invoke();
				_invokeInteract.Invoke();
			}
		}
	}
}