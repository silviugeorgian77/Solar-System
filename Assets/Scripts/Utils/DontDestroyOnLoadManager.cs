using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadManager : MonoBehaviour,
    SingletonEntry.Listener
{
    public List<Object> Objects { get; private set; }
        = new List<Object>();

    public void OnSingletonReady(bool isActiveSingleton)
    {
        
    }

    public void Add(Object o)
    {
        DontDestroyOnLoad(o);
        Objects.Add(o);
    }

    public void Remove(Object o)
    {
        Destroy(o);
        Objects.Add(o);
    }

    public void Clear()
    {
        foreach (Object o in Objects)
        {
            Remove(o);
        }
    }
}
