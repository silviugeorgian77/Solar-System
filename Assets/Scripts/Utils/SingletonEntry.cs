using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

[DisallowMultipleComponent]
public class SingletonEntry : MonoBehaviour
{
    [Tooltip("Key is optional.\n" +
        "If no  key is provided, use the GetFirst Method")]
    public string key;
    public MonoBehaviour entry;

    public bool IsActiveSingleton { get; private set; }

    private void Awake()
    {
        RegisterSingleton();
    }

    public void RegisterSingleton()
    {
        if (entry != null)
        {
            if (key == null || key.Length == 0)
            {
                key = entry.GetType().ToString();
            }
            IsActiveSingleton = Singleton.Add(key, entry);
            NotifyOnSingletonReady();
            if (IsActiveSingleton)
            {
                AddDontDestroyOnLoad();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private async void NotifyOnSingletonReady()
    {
        if (entry != null && entry is Listener listener)
        {
            while (entry != null && !entry.isActiveAndEnabled)
            {
                await Task.Yield();
            }
            if (listener != null)
            {
                listener.OnSingletonReady(IsActiveSingleton);
            }
        }
    }

    private async void AddDontDestroyOnLoad()
    {
        while (Singleton.GetFirst<DontDestroyOnLoadManager>() == null)
        {
            await Task.Yield();
        }
        Singleton.GetFirst<DontDestroyOnLoadManager>().Add(this);
    }

    public void RemoveSingleton()
    {
        Singleton.Remove(key);
        StartCoroutine(RemveDontDestroyOnLoadCouroutine());
    }

    private IEnumerator RemveDontDestroyOnLoadCouroutine()
    {
        while (Singleton.GetFirst<DontDestroyOnLoadManager>() == null)
        {
            yield return null;
        }
        Singleton.GetFirst<DontDestroyOnLoadManager>().Remove(this);
    }

    public interface Listener
    {
        void OnSingletonReady(bool isActiveSingleton);
    }
}
