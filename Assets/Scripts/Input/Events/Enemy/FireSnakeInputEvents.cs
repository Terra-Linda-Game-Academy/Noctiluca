using System;
using AI;
using Input.Data.Enemy;

namespace Input.Events.Enemy
{
	public class FireSnakeInputEvents : IInputEvents<FireSnakeInput, FireSnakeInputEvents.Dispatcher>
	{
		public event Action Attack;

		private void InvokeAttack() => Attack?.Invoke();

		public Dispatcher GetDispatcher(Func<FireSnakeInput> inputFunc)
		{
			Dispatcher dispatcher = new Dispatcher(inputFunc, InvokeAttack);
			return dispatcher;
		}

		public class Dispatcher : EventDispatcher<FireSnakeInput>
		{
			private readonly Action _invokeAttack;

			public Dispatcher(Func<FireSnakeInput> inputFunc, Action invokeAttack) : base(inputFunc)
			{
				_invokeAttack = invokeAttack;
			}

			public void Attack() => _invokeAttack?.Invoke();
		}
	}
}