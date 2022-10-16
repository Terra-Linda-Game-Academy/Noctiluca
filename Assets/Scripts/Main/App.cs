using UnityEngine;
using Object = UnityEngine.Object;

namespace Main {
    public class App {
        public static GamemodeManager GamemodeManager { get; private set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Bootstrap() {
            var app = Object.Instantiate(Resources.Load("App")) as GameObject;
            
            if (app is null) throw new AppInitException();
            Object.DontDestroyOnLoad(app);

            GamemodeManager = app.GetComponent<GamemodeManager>();

            if (GamemodeManager is null) throw new AppInitException();
        }
    }
}


