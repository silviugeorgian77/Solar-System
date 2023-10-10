using UnityEngine;
using System.Collections.Generic;

public class Cache : MonoBehaviour,
    SingletonEntry.Listener
{
    [SerializeField]
    private int count = 20;

    private List<Pair<string, object>> items
        = new List<Pair<string, object>>();

    public void OnSingletonReady(bool isActiveSingleton)
    {
        if (isActiveSingleton)
        {
            Init();
        }
    }

    private void Init()
    {

    }

    public void Add(string id, object value)
    {
        Add(new Pair<string, object>(id, value));
    }

    public void Add(Pair<string, object> item)
    {
        Remove(item.Element1);
        items.Remove(item);
        items.Add(item);
        if (items.Count > count)
        {
            items.RemoveAt(0);
        }
    }

    public void Remove(Pair<string, object> item)
    {
        items.Remove(item);
    }

    public void Remove(string id)
    {
        Pair<string, object> itemToRemove = null;
        foreach (Pair<string, object> item in items)
        {
            if (item.Element1.Equals(id))
            {
                itemToRemove = item;
            }
        }
        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
        }
    }

    public object Get(string id)
    {
        foreach (Pair<string, object> item in items)
        {
            if (item.Element1.Equals(id))
            {
                return item.Element2;
            }
        }
        return null;
    }
}
