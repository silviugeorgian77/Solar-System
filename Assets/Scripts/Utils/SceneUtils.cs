using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneUtils
{
    public static IEnumerator WaitForSceneToBeLoaded(
        string sceneName,
        List<Scene> scenesToBeUnloaded = null)
    {
        bool isSceneLoaded = false;
        while (!isSceneLoaded)
        {
            isSceneLoaded = IsSceneLoaded(sceneName, scenesToBeUnloaded);
            if (!isSceneLoaded)
            {
                yield return null;
            }
        }
    }

    public static bool IsSceneLoaded(
        string sceneName,
        List<Scene> scenesToBeUnloaded = null)
    {
        Scene scene;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            scene = SceneManager.GetSceneAt(i);
            if (scene.name.Equals(sceneName)
                && scene.GetRootGameObjects().Length > 0)
            {
                if (scenesToBeUnloaded == null
                    || !scenesToBeUnloaded.Contains(scene))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static void SetSceneInputEnabled(string sceneName, bool enabled)
    {
        Scene activeScene;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            activeScene = SceneManager.GetSceneAt(i);
            if (activeScene.name.Equals(sceneName))
            {
                TouchUtils.SetInputEventsEnabled(activeScene, enabled);
            }
        }
    }

    public static void SetOtherScenesInputEnabled(
        string excludedSceneName,
        bool enabled)
    {
        Scene activeScene;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            activeScene = SceneManager.GetSceneAt(i);
            if (!activeScene.name.Equals(excludedSceneName))
            {
                TouchUtils.SetInputEventsEnabled(activeScene, enabled);
            }
        }
    }
}
