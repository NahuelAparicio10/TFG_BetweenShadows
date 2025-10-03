using UnityEngine;
// -- CoroutineRunner makes a coroutine that can be used in NonMonoBehaciours scripts
public class CoroutineRunner : MonoBehaviour, IGameServices
{
    public static CoroutineRunner Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameServices.Register<CoroutineRunner>(this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}