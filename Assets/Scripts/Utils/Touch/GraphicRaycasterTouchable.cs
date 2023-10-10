using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class GraphicRaycasterTouchable : MonoBehaviour
{
    [SerializeField]
    private GraphicRaycaster graphicRaycaster;

    private void Awake()
    {
        if (graphicRaycaster == null)
        {
            graphicRaycaster = GetComponent<GraphicRaycaster>();
        }
        TouchUtils.AllGraphicRaycasters.Add(graphicRaycaster);
    }

    private void OnDestroy()
    {
        TouchUtils.AllGraphicRaycasters.Remove(graphicRaycaster);
    }
}
