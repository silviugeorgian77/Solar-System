using UnityEngine;

public class EaseEquations
{

    public delegate float EaseFunctionDelegate(
        float changeInValue,
        float elapsedTime,
        float duration,
        float startValue);
    public static EaseFunctionDelegate easeInFunction = easeIn;
    public static EaseFunctionDelegate easeOutFunction = easeOut;
    public static EaseFunctionDelegate easeOutExponentialFunction
        = easeOutExponential;
    public static EaseFunctionDelegate easeOutBackFunction = easeOutBack;
    public static EaseFunctionDelegate noEaseFunction = noEase;

    public static float easeIn(
        float changeInValue,
        float elapsedTime,
        float duration,
        float startValue)
    {
        elapsedTime /= duration;
        return changeInValue * elapsedTime * elapsedTime + startValue;
    }

    public static float easeOut(
        float changeInValue,
        float elapsedTime,
        float duration,
        float startValue)
    {
        elapsedTime /= duration;
        return -changeInValue * elapsedTime * (elapsedTime - 2) + startValue;
    }

    public static float easeInExponential(
        float changeInValue,
        float elapsedTime,
        float duration,
        float startValue)
    {
        return changeInValue * Mathf.Pow(2, 10 * (elapsedTime / duration - 1)) + startValue;
    }

    public static float easeOutExponential(
        float changeInValue,
        float elapsedTime,
        float duration,
        float startValue)
    {
        return changeInValue * (-Mathf.Pow(2, -10 * elapsedTime / duration) + 1) + startValue;
    }

    public static float easeOutCubic(
        float changeInValue,
        float elapsedTime,
        float duration,
        float startValue)
    {
        elapsedTime /= duration;
        elapsedTime--;
        return changeInValue * (elapsedTime * elapsedTime * elapsedTime + 1) + startValue;
    }

    public static float easeOutBack(
        float changeInValue,
        float elapsedTime,
        float duration,
        float startValue)
    {
        elapsedTime /= duration;
        float ts = elapsedTime * elapsedTime;
        float tc = ts * elapsedTime;
        return startValue + changeInValue * (-6.345f * tc * ts + 12.6425f * ts * ts - 2.2f * tc - 9.195f * ts + 6.0975f * elapsedTime);
    }

    public static float noEase(
        float changeInValue,
        float elapsedTime,
        float duration,
        float startValue)
    {
        return changeInValue * elapsedTime / duration + startValue;
    }

}
