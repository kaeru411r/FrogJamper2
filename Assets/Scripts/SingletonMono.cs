using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    static T instance;

    virtual protected bool isDestroyOnLoad { get { return false; } }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    Debug.LogError($"{typeof(T)}ÇÕå©Ç¬Ç©ÇËÇ‹ÇπÇÒÇ≈ÇµÇΩÅB");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        Initialize();
    }

    protected bool Initialize()
    {
        if (instance == null)
        {
            instance = (T)this;

            if (isDestroyOnLoad)
            {
                DontDestroyOnLoad(instance);
            }

            return true;
        }
        else if(instance == this)
        {
            return true;
        }
        else
        {
            Destroy(this);
            return false;
        }
    }
}

