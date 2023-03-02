using System;

namespace Main
{
    public class AppInitException : Exception
    {
        private readonly string msg;
        public override string Message => $"App failed to initialize: {msg}";

        public AppInitException(string msg) { this.msg = msg; }
    }
}