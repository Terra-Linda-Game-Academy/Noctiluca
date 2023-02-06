using System;
using AI;
using Input.Data.Enemy;

namespace Input.Events.Enemy {
	public class SkinkEnemyInputEvents : IInputEvents<SkinkEnemyInput, SkinkEnemyInputEvents.Dispatcher> {
		public event Action Attack;

		private void InvokeAttack() => Attack?.Invoke();

		public Dispatcher GetDispatcher(Func<SkinkEnemyInput> inputFunc) {
			Dispatcher dispatcher = new Dispatcher(inputFunc, InvokeAttack);
			return dispatcher;
		}

		public class Dispatcher : EventDispatcher<SkinkEnemyInput> {
			private readonly Action _invokeAttack;

			public Dispatcher(Func<SkinkEnemyInput> inputFunc, Action invokeAttack) : base(inputFunc) {
				_invokeAttack = invokeAttack;
			}

			public void Attack() => _invokeAttack?.Invoke();
		}
	}
}