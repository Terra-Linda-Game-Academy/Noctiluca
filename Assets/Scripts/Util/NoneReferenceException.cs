using System;

namespace Util
{
    public class NoneReferenceException : Exception
    {
        private readonly Type type;
        public override string Message => $"Attempted to reference the value of a disabled Option<{type}>";

        public NoneReferenceException(Type type) { this.type = type; }
    }
}