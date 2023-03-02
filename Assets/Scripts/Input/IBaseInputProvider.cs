using System;
using System.Collections.Generic;

namespace Input
{
    public interface IBaseInputProvider
    {
        public IEnumerable<Type> GetValidMiddlewareTypes();
    }
}