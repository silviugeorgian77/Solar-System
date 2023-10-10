using UnityEngine;

[ExecuteInEditMode]
public class ChangeLayerInChildren : MonoBehaviour
{
    public string layerName;

    private int layer;
    private string currentLayerName;

    private void Awake()
    {
        ChangeLayers();
    }

    private void Update()
    {
        if (!layerName.Equals(currentLayerName))
        {
            currentLayerName = layerName;
            layer = LayerMask.NameToLayer(currentLayerName);
        }
        if (gameObject.layer != layer)
        {
            ChangeLayers();
        }
    }

    private void ChangeLayers()
    {
        if (layerName == null || layerName.Equals(""))
        {
            return;
        }
        LayerUtils.ChangeLayersRecursively(gameObject, layerName);
    }
}
