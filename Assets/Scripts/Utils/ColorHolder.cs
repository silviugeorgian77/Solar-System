using UnityEngine;
using System;

public class ColorHolder
{
    public Color ColorInit { get; private set; }
    public Color ColorStart { get; set; }

    public bool ColorAnimationEnabled { get; set; }
    public Color ColorCurrent { get; private set; }
    public Color ColorFinal { get; set; }
    public float ColorDuration { get; set; }
    public AnimationCurve ColorAnimationCurve { get; set; }

    public Action<ColorHolder> OnColorFinished { get; set; }

    private float dolorTime;
    private float dolorTimeRatio;

    public ColorHolder(Color Color)
    {
        ColorInit = Color;
        ColorStart = Color;
        ColorCurrent = Color;
    }

    public void Update()
    {
        if (ColorAnimationEnabled)
        {
            dolorTime += Time.deltaTime;
            dolorTimeRatio = dolorTime / ColorDuration;
            ColorCurrent = Color.Lerp(
                ColorStart,
                ColorFinal,
                ColorAnimationCurve.Evaluate(dolorTimeRatio)
            );
            if (dolorTime > ColorDuration)
            {
                ColorCurrent = ColorFinal;
                ColorStart = ColorFinal;
                dolorTime = 0;
                ColorAnimationEnabled = false;
                OnColorFinished?.Invoke(this);
            }
        }
    }
}
