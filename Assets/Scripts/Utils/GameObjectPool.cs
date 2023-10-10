using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class GameObjectPool : MonoBehaviour,
    SingletonEntry.Listener
{
    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab
    {
        get
        {
            return prefab;
        }
        set
        {
            prefab = value;
        }
    }

    [SerializeField]
    private GameObject[] preCookedInstances;

    [SerializeField]
    private int initMaxCount = 10;
    public int InitMaxCount
    {
        get
        {
            return initMaxCount;
        }
        set
        {
            initMaxCount = value;
        }
    }

    [SerializeField]
    private bool autoInit = true;

    [SerializeField]
    private bool deactivateUnusedOnInit = true;
    public bool DeactivateUnusedOnInit
    {
        get
        {
            return deactivateUnusedOnInit;
        }
        set
        {
            deactivateUnusedOnInit = value;
        }
    }

    private List<GameObject> unusedInstances = new List<GameObject>();
    public ReadOnlyCollection<GameObject> UnusedInstances
    {
        get
        {
            return unusedInstances.AsReadOnly();
        }
    }

    private List<GameObject> usedInstances = new List<GameObject>();
    public ReadOnlyCollection<GameObject> UsedInstances
    {
        get
        {
            return usedInstances.AsReadOnly();
        }
    }

    public ReadOnlyCollection<GameObject> AllInstances
    {
        get
        {
            var allInstances = new List<GameObject>();
            allInstances.AddRange(unusedInstances);
            allInstances.AddRange(usedInstances);
            return allInstances.AsReadOnly();
        }
    }

    public Action OnInitialized { get; set; }

    public Action<GameObject> OnPoolGet { get; set; }

    public Action<GameObject> OnPoolPut { get; set; }

    public bool IsInitialized { get; private set; }

    private void Awake()
    {
        if (autoInit && GetComponent<SingletonEntry>() == null)
        {
            Init();
        }
    }

    public void OnSingletonReady(bool isActiveSingleton)
    {
        if (autoInit && isActiveSingleton)
        {
            Init();
        }
    }

    public void Init()
    {
        Clear();
        unusedInstances.AddRange(preCookedInstances);

        if (prefab != null)
        {
            var neededObjectsCount = initMaxCount - unusedInstances.Count;
            for (int i = 0; i < neededObjectsCount; i++)
            {
                var instance = Instantiate(prefab, transform);
                instance.name = i.ToString();
                unusedInstances.Add(instance);
            }
        }

        if (deactivateUnusedOnInit)
        {
            foreach (var instance in unusedInstances)
            {
                instance.SetActive(false);
            }
        }

        IsInitialized = true;

        OnInitialized?.Invoke();
    }

    public GameObject Get(
        bool activate = true,
        GameObject prefferedInstance = null)
    {
        GameObject instance = null;
        if (unusedInstances.Count == 0)
        {
            instance = Instantiate(prefab, transform);
        }
        else
        {
            if (prefferedInstance != null)
            {
                for (var i = unusedInstances.Count - 1; i >= 0; i--)
                {
                    var unusedInstance = unusedInstances[i];
                    if (unusedInstance == prefferedInstance)
                    {
                        instance = unusedInstance;
                        unusedInstances.RemoveAt(i);
                        break;
                    }
                }
            }

            if (instance == null)
            {
                instance = unusedInstances[unusedInstances.Count - 1];
                unusedInstances.RemoveAt(unusedInstances.Count - 1);
            }
        }
        usedInstances.Add(instance);
        if (activate)
        {
            instance.SetActive(true);
        }
        OnPoolGet?.Invoke(instance);
        return instance;
    }

    public void Put(GameObject instance, bool deactivate = true)
    {
        if (!unusedInstances.Contains(instance))
        {
            unusedInstances.Add(instance);
        }
        usedInstances.Remove(instance);
        instance.transform.parent = transform;
        if (deactivate)
        {
            instance.SetActive(false);
        }
        OnPoolPut?.Invoke(instance);
    }

    public void Clear()
    {
        ClearUsedInstances();
        ClearUnusedInstances();
    }

    public void ClearUsedInstances()
    {
        Clear(usedInstances);
    }

    public void ClearUnusedInstances()
    {
        Clear(unusedInstances);
    }

    private void Clear(List<GameObject> instances)
    {
        foreach (var unusedInstance in instances)
        {
            Destroy(unusedInstance);
        }
        instances.Clear();
    }
}
