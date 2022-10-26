using UnityEngine;

namespace Main {
    public class App {
        public static GamemodeManager GamemodeManager { get; private set; }
        public static SaveManager SaveManager { get; private set; }
        public static InputManager InputManager { get; private set; }

        private static bool paused = false;
        public static bool Paused {
            get => paused;
            set {
                paused = value;
                if (paused) {
                    Time.timeScale = 0;
                } else {
                    Time.timeScale = 1;
                }
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Bootstrap() {
            var app = Object.Instantiate(Resources.Load("App")) as GameObject;

            if (app is null) throw new AppInitException("App prefab not found in Resources folder");
            Object.DontDestroyOnLoad(app);

            GamemodeManager = app.GetComponent<GamemodeManager>();
            if (GamemodeManager is null) 
                throw new AppInitException("App prefab missing required GameModeManager component");
            
            SaveManager = app.GetComponent<SaveManager>();
            if (SaveManager is null) 
                throw new AppInitException("App prefab missing required SaveManager component");

            InputManager = app.GetComponent<InputManager>();
            if (InputManager is null) 
                throw new AppInitException("App prefab missing required InputManager component");
        }
    }
}


