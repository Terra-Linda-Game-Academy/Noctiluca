using System;
using Input.Data.Enemy;

namespace Input.Events.Enemy
{
	public class AntInputEvents : IInputEvents<AntInput, AntInputEvents.Dispatcher>
	{
		public event Action Bite;

		private void InvokeBite() => Bite?.Invoke();

		public Dispatcher GetDispatcher(Func<AntInput> inputFunc)
		{
			return new Dispatcher(inputFunc, InvokeBite); ;
		}

		public class Dispatcher : EventDispatcher<AntInput>
		{
			private readonly Action _invokeBite;

			public Dispatcher(Func<AntInput> inputFunc, Action invokeBite) : base(inputFunc)
			{
				_invokeBite = invokeBite;
			}

			public void Bite() => _invokeBite?.Invoke();
		}
	}
}