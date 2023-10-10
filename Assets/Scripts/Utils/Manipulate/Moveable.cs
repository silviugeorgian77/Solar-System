using UnityEngine;

public class Moveable : MonoBehaviour
{
    private enum MoveMode
    {
        DIRECT,
        BEZIER_CURVE
    }

    protected bool executeMove = false;
    private Vector3 move;
    private TransformScope transformScope;
    private EaseEquations.EaseFunctionDelegate easeFunctionMove;
    private Vector3 start;
    public float durationMove;
    public float elapsedTimeMove;
    private Vector3 changeInValueMove;
    public delegate void MoveEndedCallBackFunction();
    private MoveEndedCallBackFunction MoveEndedCallBack;
    private Vector3 position;
    private MoveMode moveMode;

    private BezierCurve bezierCurve;

    public void Move(
        float newX,
        float newY,
        float newZ,
        float duration,
        TransformScope transformScope,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        MoveEndedCallBackFunction EndCallBack = null)
    {
        MoveStop();
        MoveEndedCallBack = EndCallBack;
        if (duration == 0)
        {
            position.x = newX;
            position.y = newY;
            position.z = newZ;
            if (transformScope == TransformScope.GLOBAL)
            {
                transform.position = position;
            }
            else
            {
                transform.localPosition = position;
            }
            OnFinished();
            return;
        }
        if (easeFunction == null)
        {
            easeFunction = EaseEquations.noEaseFunction;
        }
        move.x = newX;
        move.y = newY;
        move.z = newZ;
        executeMove = true;
        durationMove = duration;
        this.transformScope = transformScope;
        if (transformScope == TransformScope.GLOBAL)
        {
            start.x = transform.position.x;
            start.y = transform.position.y;
            start.z = transform.position.z;
        }
        else
        {
            start.x = transform.localPosition.x;
            start.y = transform.localPosition.y;
            start.z = transform.localPosition.z;
        }
        elapsedTimeMove = 0;
        changeInValueMove.x = newX - start.x;
        changeInValueMove.y = newY - start.y;
        changeInValueMove.z = newZ - start.z;
        easeFunctionMove = easeFunction;
        moveMode = MoveMode.DIRECT;
    }

    public void MoveX(
        float newX,
        float duration,
        TransformScope transformScope,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        MoveEndedCallBackFunction EndCallBack = null)
    {
        MoveStop();
        MoveEndedCallBack = EndCallBack;
        if (duration == 0)
        {
            position.x = newX;
            if (transformScope == TransformScope.GLOBAL)
            {
                position.y = transform.position.y;
                position.z = transform.position.z;
                transform.position = position;
            }
            else
            {
                position.y = transform.localPosition.y;
                position.z = transform.localPosition.z;
                transform.localPosition = position;
            }
            OnFinished();
        }
        if (easeFunction == null)
        {
            easeFunction = EaseEquations.noEaseFunction;
        }
        move.x = newX;
        executeMove = true;
        durationMove = duration;
        this.transformScope = transformScope;
        if (transformScope == TransformScope.GLOBAL)
        {
            start.x = transform.position.x;
            start.y = transform.position.y;
            start.z = transform.position.z;
            move.y = transform.position.y;
            move.z = transform.position.z;
        }
        else
        {
            start.x = transform.localPosition.x;
            start.y = transform.localPosition.y;
            start.z = transform.localPosition.z;
            move.y = transform.localPosition.y;
            move.z = transform.localPosition.z;
        }
        elapsedTimeMove = 0;
        changeInValueMove.x = newX - start.x;
        changeInValueMove.y = 0;
        changeInValueMove.z = 0;
        easeFunctionMove = easeFunction;
        moveMode = MoveMode.DIRECT;
    }

    public void MoveY(
        float newY,
        float duration,
        TransformScope transformScope,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        MoveEndedCallBackFunction EndCallBack = null)
    {
        MoveStop();
        MoveEndedCallBack = EndCallBack;
        if (duration == 0)
        {
            position.y = newY;
            if (transformScope == TransformScope.GLOBAL)
            {
                position.x = transform.position.x;
                position.z = transform.position.z;
                transform.position = position;
            }
            else
            {
                position.x = transform.localPosition.x;
                position.z = transform.localPosition.z;
                transform.localPosition = position;
            }
            OnFinished();
        }
        if (easeFunction == null)
        {
            easeFunction = EaseEquations.noEaseFunction;
        }
        move.y = newY;
        executeMove = true;
        durationMove = duration;
        this.transformScope = transformScope;
        if (transformScope == TransformScope.GLOBAL)
        {
            start.x = transform.position.x;
            start.y = transform.position.y;
            start.z = transform.position.z;
            move.x = transform.position.x;
            move.z = transform.position.z;
        }
        else
        {
            start.x = transform.localPosition.x;
            start.y = transform.localPosition.y;
            start.z = transform.localPosition.z;
            move.x = transform.localPosition.x;
            move.z = transform.localPosition.z;
        }
        elapsedTimeMove = 0;
        changeInValueMove.x = 0;
        changeInValueMove.y = newY - start.y;
        changeInValueMove.z = 0;
        easeFunctionMove = easeFunction;
        moveMode = MoveMode.DIRECT;
    }

    public void MoveZ(
        float newZ,
        float duration,
        TransformScope transformScope,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        MoveEndedCallBackFunction EndCallBack = null)
    {
        MoveStop();
        MoveEndedCallBack = EndCallBack;
        if (duration == 0)
        {
            position.z = newZ;
            if (transformScope == TransformScope.GLOBAL)
            {
                position.x = transform.position.x;
                position.y = transform.position.y;
                transform.position = position;
            }
            else
            {
                position.x = transform.localPosition.x;
                position.y = transform.localPosition.y;
                transform.localPosition = position;
            }
            OnFinished();
        }
        if (easeFunction == null)
        {
            easeFunction = EaseEquations.noEaseFunction;
        }
        move.z = newZ;
        executeMove = true;
        durationMove = duration;
        this.transformScope = transformScope;
        if (transformScope == TransformScope.GLOBAL)
        {
            start.x = transform.position.x;
            start.y = transform.position.y;
            start.z = transform.position.z;
            move.x = transform.position.x;
            move.y = transform.position.y;
        }
        else
        {
            start.x = transform.localPosition.x;
            start.y = transform.localPosition.y;
            start.z = transform.localPosition.z;
            move.x = transform.localPosition.x;
            move.y = transform.localPosition.y;
        }
        elapsedTimeMove = 0;
        changeInValueMove.x = 0;
        changeInValueMove.y = 0;
        changeInValueMove.z = newZ - start.z;
        easeFunctionMove = easeFunction;
        moveMode = MoveMode.DIRECT;
    }

    public void MoveOnBezierCurve(
        BezierCurve bezierCurve,
        float duration,
        TransformScope transformScope,
        MoveEndedCallBackFunction EndCallBack = null)
    {
        MoveEndedCallBack = EndCallBack;
        executeMove = true;
        move.x = bezierCurve.ControlPoints[3].x;
        move.y = bezierCurve.ControlPoints[3].y;
        move.z = bezierCurve.ControlPoints[3].z;
        durationMove = duration;
        this.bezierCurve = bezierCurve;
        this.transformScope = transformScope;
        elapsedTimeMove = 0;
        moveMode = MoveMode.BEZIER_CURVE;
    }

    public void MoveStop()
    {
        executeMove = false;
    }

    protected virtual void Update()
    {
        if (executeMove)
        {
            elapsedTimeMove += Time.deltaTime;
            if (moveMode == MoveMode.DIRECT)
            {
                position.x = easeFunctionMove(
                    changeInValueMove.x,
                    elapsedTimeMove,
                    durationMove,
                    start.x);
                position.y = easeFunctionMove(
                    changeInValueMove.y,
                    elapsedTimeMove,
                    durationMove,
                    start.y);
                position.z = easeFunctionMove(
                    changeInValueMove.z,
                    elapsedTimeMove,
                    durationMove,
                    start.z);
            }
            else if (moveMode == MoveMode.BEZIER_CURVE
                && bezierCurve != null)
            {
                position = bezierCurve.GetParametricPointAtTime(
                    elapsedTimeMove / durationMove
                );
            }

            if (float.IsNaN(position.x)
                    || float.IsInfinity(position.x))
            {
                position.x = 0;
            }
            if (float.IsNaN(position.y)
                || float.IsInfinity(position.y))
            {
                position.y = 0;
            }
            if (float.IsNaN(position.z)
                || float.IsInfinity(position.z))
            {
                position.z = 0;
            }

            if (transformScope == TransformScope.GLOBAL)
            {
                transform.position = position;
            }
            else
            {
                transform.localPosition = position;
            }

            if (elapsedTimeMove >= durationMove)
            {
                executeMove = false;
                position.x = move.x;
                position.y = move.y;
                position.z = move.z;
                if (transformScope == TransformScope.GLOBAL)
                {
                    transform.position = position;
                }
                else
                {
                    transform.localPosition = position;
                }
                OnFinished();
            }
        }
    }

    private void OnFinished()
    {
        MoveEndedCallBack?.Invoke();
    }
}
