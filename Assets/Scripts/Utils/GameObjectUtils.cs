using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameObjectUtils
{
    public static GameObject FindGameObjectByName(
        string name,
        GameObject[] gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Equals(name))
            {
                return gameObject;
            }
        }
        return null;
    }

    public static GameObject FindGameObjectRecursivelyByName(
       string name)
    {
        Scene scene;
        GameObject gameObject;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            scene = SceneManager.GetSceneAt(i);
            gameObject = FindGameObjectRecursivelyByName(name, scene);
            if (gameObject != null)
            {
                return gameObject;
            }
        }
        return null;
    }

    public static GameObject FindGameObjectRecursivelyByName(
       string name,
       Scene scene)
    {
        GameObject gameObject;
        foreach (GameObject rootObject in  scene.GetRootGameObjects())
        {
            gameObject = FindGameObjectRecursivelyByName(name, rootObject);
            if (gameObject != null)
            {
                return gameObject;
            }
        }
        return null;
    }

    public static GameObject FindGameObjectRecursivelyByName(
        string name,
        GameObject gameObject)
    {
        Transform[] allChildTransforms
            = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform childTransform in allChildTransforms)
        {
            if (childTransform.name.Equals(name))
            {
                return childTransform.gameObject;
            }
        }
        return null;
    }

    public static List<GameObject> FindGameObjectsRecursivelyByName(
        string name,
        GameObject gameObject)
    {
        List<GameObject> result = new List<GameObject>();
        Transform[] allChildTransforms
            = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform childTransform in allChildTransforms)
        {
            if (childTransform.name.Equals(name))
            {
                result.Add(childTransform.gameObject);
            }
        }
        return result;
    }
}
