using System;

namespace Main {
    public class AppInitException : Exception {
        public override string Message => "App failed to initialize";
    }
}