

using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseObjectPool<T> : MonoBehaviour where T : Component, IPoolable
{
    [Header("Pool Settings")]
    [SerializeField] protected T prefab;
    [SerializeField] protected int defaultCapacity = 10;
    [SerializeField] protected int maxPoolSize = 20;
    [SerializeField] protected bool collectionCheck = true;
    private int _activeCount;
    
    protected IObjectPool<T> objectPool;
    
    public int CountAll => _activeCount;
    public int CountActive => _activeCount + CountInactive;
    public int CountInactive => objectPool.CountInactive;
    
    // Optional Singleton pattern for specific Pools
    public static BaseObjectPool<T> Instance { get; private set; }
    

    
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    protected virtual void InitializePool()
    {
        objectPool = new ObjectPool<T>(
            createFunc: CreatePooledObject,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: collectionCheck,
            defaultCapacity: defaultCapacity,
            maxSize: maxPoolSize
        );
    }
    
    protected virtual T CreatePooledObject()
    {
        var obj = Instantiate(prefab, transform);
        obj.GetComponent<IPoolable>()?.OnSpawnFromPool();
        return obj;
    }
    
    protected virtual void OnTakeFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
        obj.OnSpawnFromPool();
    }
    
    protected virtual void OnReturnToPool(T obj)
    {
        obj.OnReturnToPool();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
    }
    
    protected virtual void OnDestroyPoolObject(T obj)
    {
        if (obj != null)
            Destroy(obj.gameObject);
    }
    
    #region Public Methods

    public T Get()
    {
        _activeCount++;
        return objectPool.Get();
    }

    public void Release(T obj)
    {
        objectPool.Release(obj);
        _activeCount--;
    }
    
    public T Get(Vector3 position, Quaternion rotation)
    {
        T obj = objectPool.Get();
        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }
    
    #endregion
}
