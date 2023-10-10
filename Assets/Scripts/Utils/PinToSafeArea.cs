using System;
using UnityEngine;

/// <summary>
/// Resizes a UI element with a RectTransform to respect the safe areas of
/// the current device.
/// This is particularly useful on an iPhone X, where we have to avoid the
/// notch and the screen
/// corners.
/// 
/// The easiest way to use it is to create a root Canvas object, attach this
/// script to a game object called "SafeAreaContainer"
/// that is the child of the root canvas, and then layout the UI elements
/// within the SafeAreaContainer, which will adjust size appropriately for
/// the current device.
/// </summary>
public class PinToSafeArea : MonoBehaviour
{
    
    /// <summary>
    /// Left, Top, Right, Bottom padding
    /// </summary>
    [Tooltip("Left, Top, Right, Bottom padding")]
    public Vector4 padding;

    public Action OnSafeAreaChanged;

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private Rect lastScreenSafeArea;
    private Rect lastParentRect;
    private Vector4 lastPadding;

    private void Start()
    {
        parentRectTransform = this.GetComponentInParent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (lastScreenSafeArea != Screen.safeArea
            || lastParentRect != parentRectTransform.rect
            || lastPadding != padding)
        {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        Rect safeAreaRect = new Rect(
            Screen.safeArea.x + padding.x,
            Screen.safeArea.y + padding.y,
            Screen.safeArea.width - padding.z - padding.x,
            Screen.safeArea.height - padding.w - padding.y
        );

        float scaleRatio
            = parentRectTransform.rect.height
            / Screen.height;

        var left = safeAreaRect.xMin * scaleRatio;
        var right = -(Screen.width - safeAreaRect.xMax) * scaleRatio;
        var top = -safeAreaRect.yMin * scaleRatio;
        var bottom = (Screen.height - safeAreaRect.yMax) * scaleRatio;

        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(right, top);

        lastScreenSafeArea = Screen.safeArea;
        lastParentRect = parentRectTransform.rect;
        lastPadding = padding;

        OnSafeAreaChanged?.Invoke();
    }
}