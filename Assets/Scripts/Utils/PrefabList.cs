using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "PrefabList",
    menuName = "Prefab List"
)]
public class PrefabList : ScriptableObject
{
    public List<GameObject> list;

    public GameObject FindPrefab(string name)
    {
        foreach (GameObject gameObject in list)
        {
            if (gameObject.name.Equals(name))
            {
                return gameObject;
            }
        }

        return null;
    }
}
