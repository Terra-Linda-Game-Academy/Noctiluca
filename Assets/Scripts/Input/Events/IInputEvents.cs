using System;

namespace Input.Events
{
    public interface IInputEvents<in T, out D> where D : EventDispatcher<T>
    {
        public D GetDispatcher(Func<T> inputFunc);
    }
}