using Input.Data.Enemy;
using System;

namespace Input.Events.Enemy
{
    public class WalkingEnemyInputEvents : IInputEvents<WalkingEnemyInput, WalkingEnemyInputEvents.Dispatcher>
    {
        public event Action Attack;

        private void InvokeAttack() => Attack?.Invoke();

        public Dispatcher GetDispatcher(Func<WalkingEnemyInput> inputFunc)
        {
            Dispatcher dispatcher = new Dispatcher(inputFunc, InvokeAttack);
            return dispatcher;
        }

        public class Dispatcher : EventDispatcher<WalkingEnemyInput>
        {
            private readonly Action _invokeAttack;

            public Dispatcher(Func<WalkingEnemyInput> inputFunc, Action invokeAttack) : base(inputFunc)
            {
                _invokeAttack = invokeAttack;
            }

            public void Attack() => _invokeAttack?.Invoke();
        }
    }
}