using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private readonly List<Action> actions = new List<Action>();

    public void Update()
    {
        lock (actions)
        {
            foreach (var action in actions)
            {
                action?.Invoke();
            }
            actions.Clear();
        }
    }

    public void Add(Action action)
    {
        lock (actions)
        {
            actions.Add(action);
        }
    }

    public void Remove(Action action)
    {
        lock (actions)
        {
            actions.Remove(action);
        }
    }

    public void Clear()
    {
        lock (actions)
        {
            actions.Clear();
        }
    }
}
