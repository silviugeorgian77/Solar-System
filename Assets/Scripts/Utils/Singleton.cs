using System.Collections.Generic;

public class Singleton
{
    private static Dictionary<string, object> singletons
        = new Dictionary<string, object>();

    public static T Get<T>(string key)
    {
        if (singletons.ContainsKey(key))
        {
            return (T) singletons[key];
        }

        return default;
    }

    public static T GetFirst<T>()
    {
        foreach (object o in singletons.Values)
        {
            if (o is T)
            {
                return (T) o;
            }
        }

        return default;
    }

    public static List<T> GetAll<T>()
    {
        var tList = new List<T>();
        foreach (object o in singletons.Values)
        {
            if (o is T)
            {
                tList.Add((T)o);
            }
        }

        return tList;
    }

    /// <summary>
    /// Return true if this is the active singleton value and false if another
    /// singleton has aldready been registered.
    /// </summary>
    public static bool Add<T>(string key, T value)
    {
        if (!singletons.ContainsKey(key))
        {
            singletons.Add(key, value);
            return true;
        }
        return false;
    }

    public static void Remove(string key)
    {
        singletons.Remove(key);
    }

    public static void RemveFirst<T>()
    {
        string keyToRemove = null;
        foreach (KeyValuePair<string, object> pair in singletons)
        {
            if (pair.Value is T)
            {
                keyToRemove = pair.Key;
            }
        }
        if (keyToRemove != null)
        {
            singletons.Remove(keyToRemove);
        }
    }
}
