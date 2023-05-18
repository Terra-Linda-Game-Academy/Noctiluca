using Input.Data;
using System;

<<<<<<< HEAD
namespace Input.Events
{
    public class PlayerInputEvents : IInputEvents<PlayerInput, PlayerInputEvents.Dispatcher>
    {
        public event Action Interact;

        private void InvokeInteract() => Interact?.Invoke();

        public Dispatcher GetDispatcher(Func<PlayerInput> inputFunc)
        {
            Dispatcher dispatcher = new Dispatcher(inputFunc, InvokeInteract);
            return dispatcher;
        }

        public class Dispatcher : EventDispatcher<PlayerInput>
        {
            private readonly Action _invokeInteract;

            public Dispatcher(Func<PlayerInput> inputFunc, Action invokeInteract) : base(inputFunc)
            {
                _invokeInteract = invokeInteract;
            }

            public void Interact()
            {
                //demo event blocking, cant interact while moving
                PlayerInput input = GetInput();
                if (input.Movement.magnitude < .2) _invokeInteract?.Invoke();
            }
        }
    }
=======
namespace Input.Events {
	public class PlayerInputEvents : IInputEvents<PlayerInput, PlayerInputEvents.Dispatcher> {
		public event Action Interact;
		public event Action Attack;

		private void InvokeInteract() => Interact?.Invoke();
		private void InvokeAttack()   => Attack?.Invoke();

		public Dispatcher GetDispatcher(Func<PlayerInput> inputFunc) {
			Dispatcher dispatcher = new Dispatcher(inputFunc, InvokeInteract, InvokeAttack);
			return dispatcher;
		}

		public class Dispatcher : EventDispatcher<PlayerInput> {
			private readonly Action _invokeInteract;
			private readonly Action _invokeAttack;

			public Dispatcher(Func<PlayerInput> inputFunc, Action invokeInteract, Action invokeAttack) :
				base(inputFunc) {
				_invokeInteract = invokeInteract;
				_invokeAttack   = invokeAttack;
			}

			public void Interact() {
				//demo event blocking, cant interact while moving
				PlayerInput input = GetInput();
				if (input.Movement.magnitude < .2) _invokeInteract?.Invoke();
			}

			public void Attack() { _invokeAttack?.Invoke(); }
		}
	}
>>>>>>> main
}