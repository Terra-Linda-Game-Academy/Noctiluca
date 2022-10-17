using UnityEngine;

namespace Main {
    public class App {
        public static GameModeManager GameModeManager { get; private set; }
        public static SaveManager SaveManager { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Bootstrap() {
            var app = Object.Instantiate(Resources.Load("App")) as GameObject;

            if (app is null) throw new AppInitException("App prefab not found in Resources folder");
            Object.DontDestroyOnLoad(app);

            GameModeManager = app.GetComponent<GameModeManager>();
            if (GameModeManager is null) 
                throw new AppInitException("App prefab missing required GameModeManager component");
            
            SaveManager = app.GetComponent<SaveManager>();
            if (SaveManager is null) 
                throw new AppInitException("App prefab missing required SaveManager component");
        }
    }
}


