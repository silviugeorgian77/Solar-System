using UnityEngine;

public class Scalable : MonoBehaviour
{
    protected bool executeScale = false;
    private float finalScaleX, finalScaleY, finalScaleZ;
    public float durationScale;
    private float startScaleX, startScaleY, startScaleZ;
    private float elapsedTimeScale;
    private float changeInValueScaleX, changeInValueScaleY, changeInValueScaleZ;
    private EaseEquations.EaseFunctionDelegate easeFunctionScale;
    public delegate void ScaleEndedCallBackFunction();
    private ScaleEndedCallBackFunction ScaleEndedCallBack;
    private float scaleX, scaleY, scaleZ;
    private Vector3 scale = new Vector3();

    public void ScaleTo(
        float newScaleX,
        float newScaleY,
        float newScaleZ,
        float duration,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        ScaleEndedCallBackFunction EndCallBack = null)
    {
        ScaleStop();
        ScaleEndedCallBack = EndCallBack;
        if (duration == 0)
        {
            scale.x = newScaleX;
            scale.y = newScaleY;
            scale.z = newScaleZ;
            transform.localScale = scale;
            OnFinished();
            return;
        }
        if (easeFunction == null)
        {
            easeFunction = EaseEquations.noEaseFunction;
        }
        finalScaleX = newScaleX;
        finalScaleY = newScaleY;
        finalScaleZ = newScaleZ;
        executeScale = true;
        durationScale = duration;
        startScaleX = transform.localScale.x;
        startScaleY = transform.localScale.y;
        startScaleZ = transform.localScale.z;
        elapsedTimeScale = 0;
        changeInValueScaleX = finalScaleX - startScaleX;
        changeInValueScaleY = finalScaleY - startScaleY;
        changeInValueScaleZ = finalScaleZ - startScaleZ;
        easeFunctionScale = easeFunction;
    }

    public void ScaleStop()
    {
        executeScale = false;
    }

    private void Update()
    {
        if (executeScale)
        {
            elapsedTimeScale += Time.deltaTime;
            scaleX = easeFunctionScale(
                changeInValueScaleX,
                elapsedTimeScale,
                durationScale,
                startScaleX
            );
            scaleY = easeFunctionScale(
                changeInValueScaleY,
                elapsedTimeScale,
                durationScale,
                startScaleY
            );
            scaleZ = easeFunctionScale(
                changeInValueScaleZ,
                elapsedTimeScale,
                durationScale,
                startScaleZ
            );
            scale.x = scaleX;
            scale.y = scaleY;
            scale.z = scaleZ;
            transform.localScale = scale;
            if (elapsedTimeScale >= durationScale)
            {
                executeScale = false;
                scale.x = finalScaleX;
                scale.y = finalScaleY;
                scale.z = finalScaleZ;
                transform.localScale = scale;
                OnFinished();
            }
        }
    }

    private void OnFinished()
    {
        ScaleEndedCallBack?.Invoke();
    }
}
