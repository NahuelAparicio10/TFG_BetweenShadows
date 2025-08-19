using UnityEngine;

public static class GameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // --- CoroutineRunner ---
        var runnerGo = new GameObject("CoroutineRunner");
        var runner = runnerGo.AddComponent<CoroutineRunner>();
        GameServices.Register<CoroutineRunner>(runner);
        Object.DontDestroyOnLoad(runnerGo);

        // --- TimeScaleManager ---
        var timeGo = new GameObject("TimeScaleManager");
        var timeManager = timeGo.AddComponent<TimeScaleManager>();
        GameServices.Register<TimeScaleManager>(timeManager);
        Object.DontDestroyOnLoad(timeGo);
        
    }
}
