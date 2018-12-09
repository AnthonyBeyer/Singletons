using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static object _lock = new object();
    private static bool isShuttingDown;
    private static bool shouldSelfCreate;
    private bool isCreated;

    public static T Instance
    {
        get
        {
            lock(_lock)
            {
                if (_instance == null)
                {
                    _instance = Object.FindObjectOfType<T>();

                    if (_instance == null && !isShuttingDown)
                    {
                        shouldSelfCreate = true;
                        GameObject go = new GameObject(typeof(T).ToString());
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }

    public static bool IsAvailable()
    {
        return _instance == null ? false : true;
    }

    protected virtual void OnApplicationQuit() 
    { 
        isShuttingDown = true; 
    }

    protected virtual bool IsDontDestroyOnLoad() 
    { 
        return true;
    }

    protected virtual void Awake()
    {
        if(_instance == null) 
        { 
            // Instance was already in scene on Awake
            _instance = this as T; 
        }
        else
        {
            // Duplicate Singleton
            if (_instance != this as T)
            {
                if (Application.isPlaying)
                    Destroy(gameObject);
                else
                    DestroyImmediate(gameObject);
                return;
            }
        }

        if (IsDontDestroyOnLoad())
        {
            _instance.transform.SetParent(null);
            _instance.transform.SetAsFirstSibling();
            DontDestroyOnLoad(gameObject); 
        }

        _SelfCreate();
        Init();
    }

    private void _SelfCreate()
    {
        if (shouldSelfCreate && !isCreated)
        {
            Create();
            isCreated = true;
            shouldSelfCreate = true;
        }
    }

    /// <summary>
    /// Just some sort of fake method to force a call of "Instance"
    /// </summary>
    public static bool Warmup()
    {
        return Instance != null;
    }

    /// <summary>
    /// Use this to add additional components and setup singleton just before the first Awake firt Init (this is private)
    /// </summary>
    protected virtual void Create()
    {
        
    }

    /// <summary>
    /// it's called once after Create(), basically to initialize some variables (this is also public, for later recall)
    /// </summary>
    public virtual void Init()
    {
        
    }

}
