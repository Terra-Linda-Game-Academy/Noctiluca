using System;
using Input.Data.Enemy;

namespace Input.Events.Enemy {
	public class MushroomEnemyInputEvents : IInputEvents<MushroomEnemyInput, MushroomEnemyInputEvents.Dispatcher> {
		public event Action Attack;

		private void InvokeAttack() => Attack?.Invoke();

		public Dispatcher GetDispatcher(Func<MushroomEnemyInput> inputFunc) {
			return new Dispatcher(inputFunc, InvokeAttack);
		}

		public class Dispatcher : EventDispatcher<MushroomEnemyInput> {
			private readonly Action _invokeAttack;

			public Dispatcher(Func<MushroomEnemyInput> inputFunc, Action invokeAttack) : base(inputFunc) {
				_invokeAttack = invokeAttack;
			}

			public void Attack() => _invokeAttack?.Invoke();
		}
	}
}