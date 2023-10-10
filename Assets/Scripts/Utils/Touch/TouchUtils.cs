using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class TouchUtils : MonoBehaviour
{
    public static List<Touchable> AllTouchables
        { get; private set; }
        = new List<Touchable>();
    public static List<GraphicRaycaster> AllGraphicRaycasters
        { get; private set; }
        = new List<GraphicRaycaster>();

    public static void SetInputEventsEnabled(
       Scene scene,
       bool enabled)
    {
        SetTouchableInputEventsEnabled(scene, enabled);
        SetGraphicRaycasterInputEventsEnabled(scene, enabled);
    }

    private static void SetTouchableInputEventsEnabled(
        Scene scene,
        bool enabled)
    {
        foreach (Touchable touchable in AllTouchables)
        {
            if (touchable != null
                && touchable.gameObject.scene.Equals(scene))
            {
                touchable.InputEnabled = enabled;
            }
        }
    }

    private static void SetGraphicRaycasterInputEventsEnabled(
        Scene scene,
        bool enabled)
    {
        foreach (GraphicRaycaster graphicRaycaster
            in AllGraphicRaycasters)
        {
            if (graphicRaycaster != null
                && graphicRaycaster.gameObject.scene.Equals(scene))
            {
                graphicRaycaster.enabled = enabled;
            }
        }
    }
}
