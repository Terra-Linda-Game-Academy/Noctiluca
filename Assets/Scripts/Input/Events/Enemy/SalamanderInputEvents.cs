using Input.Data.Enemy;
using System;

namespace Input.Events.Enemy
{
    public class SalamanderInputEvents : IInputEvents<SalamanderInput, SalamanderInputEvents.Dispatcher>
    {
        public event Action Attack;

        private void InvokeAttack() => Attack?.Invoke();

        public Dispatcher GetDispatcher(Func<SalamanderInput> inputFunc)
        {
            Dispatcher dispatcher = new Dispatcher(inputFunc, InvokeAttack);
            return dispatcher;
        }

        public class Dispatcher : EventDispatcher<SalamanderInput>
        {
            private readonly Action _invokeAttack;

            public Dispatcher(Func<SalamanderInput> inputFunc, Action invokeAttack) : base(inputFunc)
            {
                _invokeAttack = invokeAttack;
            }

            public void Attack() => _invokeAttack?.Invoke();
        }
    }
}