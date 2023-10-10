using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseUIUtils
{
    public static bool IsMouseOverUIWithIgnores()
    {
        var raycastResults = GetUIElementsAtCurrentMousePosition();

        for (int i = raycastResults.Count - 1; i >= 0; i--)
        {
            if (raycastResults[i]
                .gameObject
                .GetComponent<MouseUIClickThrough>()
                != null)
            {
                raycastResults.RemoveAt(i);
            }
        }

        return raycastResults.Count > 0;
    }

    public static bool IsMouseOverUIElement(string name)
    {
        var raycastResults = GetUIElementsAtCurrentMousePosition();

        foreach (var raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.name == name)
            {
                return true;
            }
            if (raycastResult
                .gameObject
                .transform
                .FindParentRecursive(name))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsMouseOverUIElements(List<string> names)
    {
        var raycastResults = GetUIElementsAtCurrentMousePosition();

        foreach (var raycastResult in raycastResults)
        {
            foreach (var name in names)
            {
                if (raycastResult.gameObject.name == name)
                {
                    return true;
                }
                if (raycastResult
                        .gameObject
                        .transform
                        .FindParentRecursive(name))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static List<RaycastResult> GetUIElementsAtCurrentMousePosition()
    {
        PointerEventData pointerEventData
            = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        return raycastResults;
    }
}
