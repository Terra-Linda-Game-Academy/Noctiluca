using System;
using Input.Data.Enemy;

namespace Input.Events.Enemy {
	public class DracoInputEvents : IInputEvents<DracoInput, DracoInputEvents.Dispatcher> {
		public event Action Shoot;

		private void InvokeShoot() => Shoot?.Invoke();

		public Dispatcher GetDispatcher(Func<DracoInput> inputFunc) {
			return new Dispatcher(inputFunc, InvokeShoot);;
		}

		public class Dispatcher : EventDispatcher<DracoInput> {
			private readonly Action _invokeShoot;

			public Dispatcher(Func<DracoInput> inputFunc, Action invokeShoot) : base(inputFunc) {
				_invokeShoot = invokeShoot;
			}

			public void Shoot() => _invokeShoot?.Invoke();
		}
	}
}