using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ValueAnimator : MonoBehaviour
{
    private List<AnimatedValue> animatedValues = new List<AnimatedValue>();

    public AnimatedValue AnimateValue(
        float startValue,
        float endValue,
        float duration,
        Action<AnimatedValue> callback,
        Action<AnimatedValue> endCallback = null,
        EaseEquations.EaseFunctionDelegate easeFunction = null)
    {
        if (easeFunction == null)
        {
            easeFunction = EaseEquations.noEaseFunction;
        }
        var animatedValue = new AnimatedValue()
        {
            startValue = startValue,
            endValue = endValue,
            duration = duration,
            callback = callback,
            endCallback = endCallback,
            easeFunction = easeFunction,
            changeInValue = endValue - startValue,
            shouldExecute = true
        };
        if (duration == 0)
        {
            animatedValue.shouldExecute = false;
            animatedValue.currentValue = endValue;
            callback?.Invoke(animatedValue);
            endCallback?.Invoke(animatedValue);
        }
        else
        {
            animatedValues.Add(animatedValue);
        }

        return animatedValue;
    }

    public void PauseAnimateValue(AnimatedValue animatedValue)
    {
        animatedValue.shouldExecute = false;
    }

    public void ResumeAnimateValue(AnimatedValue animatedValue)
    {
        animatedValue.shouldExecute = true;
    }

    public void StopAnimateValue(AnimatedValue animatedValue)
    {
        animatedValue.shouldExecute = false;
        animatedValues.Remove(animatedValue);
    }

    private void Update()
    {
        for (var i = animatedValues.Count - 1; i >= 0; i--)
        {
            var animatedValue = animatedValues[i];
            if (animatedValue.shouldExecute)
            {
                animatedValue.elapsedTime += Time.deltaTime;
                if (animatedValue.elapsedTime < animatedValue.duration)
                {
                    animatedValue.currentValue = animatedValue.easeFunction(
                        animatedValue.changeInValue,
                        animatedValue.elapsedTime,
                        animatedValue.duration,
                        animatedValue.startValue
                    );
                    animatedValue.callback?.Invoke(animatedValue);
                }
                else
                {
                    animatedValue.currentValue = animatedValue.endValue;
                    animatedValue.shouldExecute = false;
                    animatedValues.Remove(animatedValue);
                    animatedValue.callback?.Invoke(animatedValue);
                    animatedValue.endCallback?.Invoke(animatedValue);
                }
            }
        }
    }

    public class AnimatedValue
    {
        public float startValue;
        public float endValue;
        public float duration;
        public Action<AnimatedValue> callback;
        public Action<AnimatedValue> endCallback;
        public EaseEquations.EaseFunctionDelegate easeFunction;
        public bool shouldExecute;
        public float elapsedTime;
        public float changeInValue;
        public float currentValue;
    }
}
