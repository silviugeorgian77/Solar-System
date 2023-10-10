using UnityEngine;

public class Rotatable : MonoBehaviour
{
    protected bool executeRotate = false;
    private float finalRotationX;
    private float finalRotationY;
    private float finalRotationZ;
    public float durationRotation;
    private float startRotationX;
    private float startRotationY;
    private float startRotationZ;
    private float elapsedTimeRotation;
    private float changeInValueRotationX;
    private float changeInValueRotationY;
    private float changeInValueRotationZ;
    private EaseEquations.EaseFunctionDelegate easeFunctionRotate;
    public delegate void RotateEndedCallBackFunction();
    private RotateEndedCallBackFunction RotateEndedCallBack;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    private TransformScope transformScope;

    public enum RotateMode
    {
        LONG,
        SHORT
    }

    public void RotateTo(
        float newRotationX,
        float newRotationY,
        float newRotationZ,
        float duration,
        RotateMode rotateMode = RotateMode.LONG,
        TransformScope transformScope = TransformScope.GLOBAL,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        RotateEndedCallBackFunction EndCallBack = null)
    {
        RotateStop();
        RotateEndedCallBack = EndCallBack;
        if (duration == 0)
        {
            if (transformScope == TransformScope.GLOBAL)
            {
                transform.rotation = Quaternion.Euler(
                    newRotationX,
                    newRotationY,
                    newRotationZ
                );
            }
            else
            {
                transform.localRotation = Quaternion.Euler(
                   newRotationX,
                   newRotationY,
                   newRotationZ
               );
            }
            OnFinished();
            return;
        }
        if (easeFunction == null)
        {
            easeFunction = EaseEquations.noEaseFunction;
        }
        this.transformScope = transformScope;
        finalRotationX = newRotationX;
        finalRotationY = newRotationY;
        finalRotationZ = newRotationZ;
        executeRotate = true;
        durationRotation = duration;
        if (transformScope == TransformScope.GLOBAL)
        {
            startRotationX = transform.rotation.eulerAngles.x;
            startRotationY = transform.rotation.eulerAngles.y;
            startRotationZ = transform.rotation.eulerAngles.z;
        }
        else
        {
            startRotationX = transform.localRotation.eulerAngles.x;
            startRotationY = transform.localRotation.eulerAngles.y;
            startRotationZ = transform.localRotation.eulerAngles.z;
        }
        elapsedTimeRotation = 0;
        changeInValueRotationX = finalRotationX - startRotationX;
        changeInValueRotationY = finalRotationY - startRotationY;
        changeInValueRotationZ = finalRotationZ - startRotationZ;
        if (rotateMode == RotateMode.SHORT)
        {
            if (changeInValueRotationX > 180)
            {
                finalRotationX -= 360;
                changeInValueRotationX = finalRotationX - startRotationX;
            }
            if (changeInValueRotationX < -180)
            {
                finalRotationX += 360;
                changeInValueRotationX = finalRotationX - startRotationX;
            }
            if (changeInValueRotationY > 180)
            {
                finalRotationY -= 360;
                changeInValueRotationY = finalRotationY - startRotationY;
            }
            if (changeInValueRotationY < -180)
            {
                finalRotationY += 360;
                changeInValueRotationY = finalRotationY - startRotationY;
            }
            if (changeInValueRotationZ > 180)
            {
                finalRotationZ -= 360;
                changeInValueRotationZ = finalRotationZ - startRotationZ;
            }
            if (changeInValueRotationZ < -180)
            {
                finalRotationZ += 360;
                changeInValueRotationZ = finalRotationZ - startRotationZ;
            }
        }
        easeFunctionRotate = easeFunction;
    }

    public void RotateToX(
        float newRotationX,
        float duration,
        RotateMode rotateMode = RotateMode.LONG,
        TransformScope transformScope = TransformScope.GLOBAL,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        RotateEndedCallBackFunction EndCallBack = null)
    {
        RotateTo(
            newRotationX,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z,
            duration,
            rotateMode,
            transformScope,
            easeFunction,
            EndCallBack
        );
    }

    public void RotateToY(
        float newRotationY,
        float duration,
        RotateMode rotateMode = RotateMode.LONG,
        TransformScope transformScope = TransformScope.GLOBAL,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        RotateEndedCallBackFunction EndCallBack = null)
    {
        RotateTo(
            transform.rotation.eulerAngles.x,
            newRotationY,
            transform.rotation.eulerAngles.z,
            duration,
            rotateMode,
            transformScope,
            easeFunction,
            EndCallBack
        );
    }

    public void RotateToZ(
       float newRotationZ,
       float duration,
       RotateMode rotateMode = RotateMode.LONG,
       TransformScope transformScope = TransformScope.GLOBAL,
       EaseEquations.EaseFunctionDelegate easeFunction = null,
       RotateEndedCallBackFunction EndCallBack = null)
    {
        RotateTo(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            newRotationZ,
            duration,
            rotateMode,
            transformScope,
            easeFunction,
            EndCallBack
        );
    }

    public void RotateBy(
        float amountX,
        float amountY,
        float amountZ,
        float duration,
        TransformScope transformScope = TransformScope.GLOBAL,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        RotateEndedCallBackFunction EndCallBack = null)
    {
        RotateStop();
        if (duration == 0)
        {
            if (transformScope == TransformScope.GLOBAL)
            {
                transform.rotation = Quaternion.Euler(
                    transform.rotation.eulerAngles.x + amountX,
                    transform.rotation.eulerAngles.y + amountY,
                    transform.rotation.eulerAngles.z + amountZ
                );
            }
            else
            {
                transform.localRotation = Quaternion.Euler(
                    transform.localRotation.eulerAngles.x + amountX,
                    transform.localRotation.eulerAngles.y + amountY,
                    transform.localRotation.eulerAngles.z + amountZ
                );
            }   
            OnFinished();
        }
        if (easeFunction == null)
        {
            easeFunction = EaseEquations.noEaseFunction;
        }
        this.transformScope = transformScope;
        if (transformScope == TransformScope.GLOBAL)
        {
            finalRotationX = transform.rotation.eulerAngles.x + amountX;
            finalRotationY = transform.rotation.eulerAngles.y + amountY;
            finalRotationZ = transform.rotation.eulerAngles.z + amountZ;
            startRotationX = transform.rotation.eulerAngles.x;
            startRotationY = transform.rotation.eulerAngles.y;
            startRotationZ = transform.rotation.eulerAngles.z;
        }
        else
        {
            finalRotationX = transform.localRotation.eulerAngles.x + amountX;
            finalRotationY = transform.localRotation.eulerAngles.y + amountY;
            finalRotationZ = transform.localRotation.eulerAngles.z + amountZ;
            startRotationX = transform.localRotation.eulerAngles.x;
            startRotationY = transform.localRotation.eulerAngles.y;
            startRotationZ = transform.localRotation.eulerAngles.z;
        }
        executeRotate = true;
        durationRotation = duration;
        elapsedTimeRotation = 0;
        changeInValueRotationX = finalRotationX - startRotationX;
        changeInValueRotationY = finalRotationY - startRotationY;
        changeInValueRotationZ = finalRotationZ - startRotationZ;
        easeFunctionRotate = easeFunction;
        RotateEndedCallBack = EndCallBack;
    }

    public void RotateByX(
        float amountX,
        float duration,
        TransformScope transformScope = TransformScope.GLOBAL,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        RotateEndedCallBackFunction EndCallBack = null)
    {
        RotateBy(
            amountX,
            0,
            0,
            duration,
            transformScope,
            easeFunction,
            EndCallBack
        );
    }

    public void RotateByY(
        float amountY,
        float duration,
        TransformScope transformScope = TransformScope.GLOBAL,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        RotateEndedCallBackFunction EndCallBack = null)
    {
        RotateBy(
            0,
            amountY,
            0,
            duration,
            transformScope,
            easeFunction,
            EndCallBack
        );
    }

    public void RotateByZ(
        float amountZ,
        float duration,
        TransformScope transformScope = TransformScope.GLOBAL,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        RotateEndedCallBackFunction EndCallBack = null)
    {
        RotateBy(
            0,
            0,
            amountZ,
            duration,
            transformScope,
            easeFunction,
            EndCallBack
        );
    }

    public void RotateStop()
    {
        executeRotate = false;
    }

    private void Update()
    {
        if (executeRotate)
        {
            elapsedTimeRotation += Time.deltaTime;
            rotationX = easeFunctionRotate(
                changeInValueRotationX,
                elapsedTimeRotation,
                durationRotation,
                startRotationX
            );
            rotationY = easeFunctionRotate(
                changeInValueRotationY,
                elapsedTimeRotation,
                durationRotation,
                startRotationY
            );
            rotationZ = easeFunctionRotate(
                changeInValueRotationZ,
                elapsedTimeRotation,
                durationRotation,
                startRotationZ
            );
            if (transformScope == TransformScope.GLOBAL)
            {
                transform.rotation = Quaternion.Euler(
                    rotationX,
                    rotationY,
                    rotationZ
                );
            }
            else
            {
                transform.localRotation = Quaternion.Euler(
                    rotationX,
                    rotationY,
                    rotationZ
                );
            }
            if (elapsedTimeRotation >= durationRotation)
            {
                if (transformScope == TransformScope.GLOBAL)
                {
                    transform.rotation = Quaternion.Euler(
                        finalRotationX,
                        finalRotationY,
                        finalRotationZ
                    );
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(
                        finalRotationX,
                        finalRotationY,
                        finalRotationZ
                    );
                }
                executeRotate = false;
                OnFinished();
            }
        }
    }

    private void OnFinished()
    {
        RotateEndedCallBack?.Invoke();
    }
}
