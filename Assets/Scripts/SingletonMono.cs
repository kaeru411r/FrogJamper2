using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    static T instance;

    virtual protected bool isDontDestroyOnLoad => false;

    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<T>();
                if(!instance)
                {
                    Debug.LogError($"{typeof(T)}は見つかりませんでした。");
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

            if (isDontDestroyOnLoad)
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

