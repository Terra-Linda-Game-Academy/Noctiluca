using UnityEngine;
using Object = UnityEngine.Object;

namespace Main {
    public class App {
        public static GamemodeManager GamemodeManager { get; private set; }
        public static SaveManager SaveManager { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Bootstrap() {
            var app = Object.Instantiate(Resources.Load("App")) as GameObject;

            if (app is null) throw new AppInitException("App prefab not found in Resources folder");
            Object.DontDestroyOnLoad(app);

            GamemodeManager = app.GetComponent<GamemodeManager>();
            if (GamemodeManager is null) 
                throw new AppInitException("App prefab missing required GamemodeManager component");
            
            SaveManager = app.GetComponent<SaveManager>();
            if (SaveManager is null) 
                throw new AppInitException("App prefab missing required SaveManager component");
        }
    }
}


