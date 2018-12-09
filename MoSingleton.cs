using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static object _lock = new object();
    private static bool isShuttingDown = false;
    private static bool shouldSelfCreate = false;
    private bool isCreated = false;

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
            _instance = this as T; 
        }
        else
        {
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
    /// Override this to add additional components/prefabs/stuff and setup singleton just before the first Awake/Init
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
