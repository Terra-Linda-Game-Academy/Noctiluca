using UnityEngine;
using Object = UnityEngine.Object;

namespace Main
{
    public class App
    {
        public static GameModeManager GameModeManager { get; private set; }
        public static SaveManager SaveManager { get; private set; }
        public static InputManager InputManager { get; private set; }

        private static bool paused = false;
        public static bool Paused
        {
            get => paused;
            set
            {
                paused = value;
                if (paused)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Bootstrap()
        {
            var app = Object.Instantiate(Resources.Load("App")) as GameObject;

            if (app is null) throw new AppInitException("App prefab not found in Resources folder");
            Object.DontDestroyOnLoad(app);

            GameModeManager = app.GetComponent<GameModeManager>();
            if (GameModeManager is null)
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


