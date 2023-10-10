using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(FrameWaiter))]
public class WaitForDisplay : MonoBehaviour
{
    public int framesToWait = 1;

    private FrameWaiter frameWaiter;
    private List<Renderer> renderers = new List<Renderer>();

    private void Awake()
    {
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        SetVisibility(false);

        frameWaiter = GetComponent<FrameWaiter>();
        frameWaiter.WaitForFrames(1, OnFramesPassed);
    }

    private void OnFramesPassed()
    {
        SetVisibility(true);
    }

    private void SetVisibility(bool visible)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }
}
