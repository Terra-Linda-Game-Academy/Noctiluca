using System;

namespace Input.Events {
	public class PlayerInputEvents : IInputEvents<PlayerInput, PlayerInputEvents.Dispatcher> {
		public event Action Interact;
		
		private void InvokeInteract() => Interact?.Invoke();

		public Dispatcher GetDispatcher(Func<PlayerInput> inputFunc) {
			Dispatcher dispatcher = new Dispatcher(inputFunc, InvokeInteract);
			return dispatcher;
		}

		public class Dispatcher : EventDispatcher<PlayerInput> {
			public Dispatcher(Func<PlayerInput> inputFunc, Action invokeInteract) : base(inputFunc) {
				_invokeInteract = invokeInteract;
			}

			private readonly Action _invokeInteract;

			public void Interact() {
				//demo event blocking, cant interact while moving
				PlayerInput input = GetInput();
				//if (inputData.movement.magnitude < .2) _invokeInteract?.Invoke();
				_invokeInteract.Invoke();
			}
		}
	}
}