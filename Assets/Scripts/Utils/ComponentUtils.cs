using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComponentUtils
{
    public static List<T> FindComponentsOfTypeInAllScenes<T>(
        bool includeInactive = true)
    {
        List<T> interfaces = new List<T>();

        Scene scene;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            scene = SceneManager.GetSceneAt(i);
            interfaces.AddRange(
                FindComponentsOfTypeInScene<T>(scene, includeInactive)
            );
        }

        interfaces.AddRange(FindComponentsOfTypeInDontDestroyOnLoad<T>(true));
        
        return interfaces;
    }


    public static List<T> FindComponentsOfTypeInActiveScene<T>(
        bool includeInactive = true)
    {
        Scene scene = SceneManager.GetActiveScene();
        List<T> interfaces = new List<T>();
        interfaces.AddRange(
            FindComponentsOfTypeInScene<T>(scene, includeInactive)
        );
        interfaces.AddRange(FindComponentsOfTypeInDontDestroyOnLoad<T>(true));
        return interfaces;
    }

    public static List<T> FindComponentsOfTypeInScene<T>(
        Scene scene,
        bool includeInactive = true)
    {
        GameObject[] rootGameObjects = scene.GetRootGameObjects();
        return FindComponentsOfTypeInObjects<T>(
            new List<GameObject>(rootGameObjects),
            includeInactive
        );
    }

    public static List<T> FindComponentsOfTypeInObjects<T>(
        List<GameObject> gameObjects,
        bool includeInactive = true)
    {
        List<T> interfaces = new List<T>();
        foreach (var gameObject in gameObjects)
        {
            T[] childrenInterfaces
                = gameObject.GetComponentsInChildren<T>(includeInactive);
            interfaces.AddRange(childrenInterfaces);
        }
        return interfaces;
    }

    public static List<T> FindComponentsOfTypeInObject<T>(
        GameObject gameObject,
        bool includeInactive = true)
    {
        List<T> interfaces = new List<T>();
        T[] childrenInterfaces
                = gameObject.GetComponentsInChildren<T>(includeInactive);
        interfaces.AddRange(childrenInterfaces);
        return interfaces;
    }

    public static List<T> FindComponentsOfTypeInDontDestroyOnLoad<T>(
        bool includeInactive = true)
    {
        List<T> interfaces = new List<T>();
        if (Singleton.GetFirst<DontDestroyOnLoadManager>() != null)
        {
            foreach (var o
                in Singleton.GetFirst<DontDestroyOnLoadManager>().Objects)
            {
                if (o is MonoBehaviour)
                {
                    interfaces.AddRange(
                        FindComponentsOfTypeInObject<T>(
                            ((MonoBehaviour) o).gameObject,
                            includeInactive
                        )
                    );
                }
            }
        }
        return interfaces;
    }
}
