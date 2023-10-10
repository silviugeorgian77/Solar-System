using System;
using UnityEngine;

public class Swipeable : MonoBehaviour
{
    public enum Direction
    {
        NONE,
        N,
        S,
        E,
        W,
        NE,
        NW,
        SE,
        SW
    }

    [SerializeField]
    private Touchable touchable;

    /// <summary>
    /// We allow changes by this angle amount, without considering a direction
    /// change.
    /// </summary>
    [SerializeField]
    private float directionChangeThreshold = 30f;
    public float DirectionChangeThreshold
    {
        get
        {
            return directionChangeThreshold;
        }
        set
        {
            directionChangeThreshold = value;
        }
    }

    /// <summary>
    /// Represents the minimum distance that the touch must travel
    /// during <see cref="onSwipingUpdateTimeS"/>, in order for it to
    /// be considered a swipe.
    /// </summary>
    [SerializeField]
    private float swipeDistanceThreshold = .025f;
    public float SwipeDistanceThreshold
    {
        get
        {
            return swipeDistanceThreshold;
        }
        set
        {
            swipeDistanceThreshold = value;
        }
    }

    [SerializeField]
    private float onSwipingUpdateTimeS = .05f;
    public float OnSwipingUpdateTimeS
    {
        get
        {
            return onSwipingUpdateTimeS;
        }
        set
        {
            onSwipingUpdateTimeS = value;
        }
    }

    /// <summary>
    /// The parameters in order:
    /// - last point
    /// - current point
    /// - last direction vector
    /// - direction vector
    /// - <see cref="Direction"/>
    /// - angle
    /// - distance
    /// </summary>
    public Action<
        Vector3,
        Vector3,
        Vector3,
        Direction,
        Direction,
        float,
        float,
        float
        > OnSwiping
    { get; set; }

    private Vector3 lastPoint;
    private float lastCallbackTimeS;
    private Direction lastNonNoneDirection;
    private float lastNonNoneDistance;

    private void Awake()
    {
        touchable.OnClickStartedInsideCallBack = touchable =>
        {
            lastPoint = touchable.MouseWorldPosition;
            lastCallbackTimeS = Time.time;
            lastNonNoneDirection = Direction.NONE;
            lastNonNoneDistance = 0;
        };

        touchable.OnClickHoldingCallBack = touchable =>
        {
            var currentTime = Time.time;
            var deltaTime = currentTime - lastCallbackTimeS;
            if (deltaTime < onSwipingUpdateTimeS)
            {
                return;
            }

            lastCallbackTimeS = currentTime;

            var currentPoint = touchable.MouseWorldPosition;

            var distance = Vector3.Distance(lastPoint, currentPoint);

            var directionVector
                = MathUtils.GetDirection(lastPoint, currentPoint);

            var angle = MathUtils.GetLookAtAngleXY(
                lastPoint,
                currentPoint
            );

            Direction direction = Direction.NONE;
            if (distance >= swipeDistanceThreshold)
            {
                if (angle >= 90 - directionChangeThreshold
                    && angle <= 90 + directionChangeThreshold)
                {
                    direction = Direction.N;
                }
                else if (angle >= 270 - directionChangeThreshold
                    && angle <= 270 + directionChangeThreshold)
                {
                    direction = Direction.S;
                }
                else if (angle >= -directionChangeThreshold
                    && angle <= directionChangeThreshold)
                {
                    direction = Direction.E;
                }
                else if (angle >= 180 - directionChangeThreshold
                    && angle <= 180 + directionChangeThreshold)
                {
                    direction = Direction.W;
                }
                else if (angle >= 0 && angle <= 90)
                {
                    direction = Direction.NE;
                }
                else if (angle >= 90 && angle <= 180)
                {
                    direction = Direction.NW;
                }
                else if (angle >= 180 && angle <= 270)
                {
                    direction = Direction.SW;
                }
                else if (angle >= 270 && angle <= 360)
                {
                    direction = Direction.SE;
                }
            }
            OnSwiping?.Invoke(
                lastPoint,
                currentPoint,
                directionVector,
                lastNonNoneDirection,
                direction,
                angle,
                lastNonNoneDistance,
                distance
            );

            lastPoint = currentPoint;
            if (direction != Direction.NONE)
            {
                lastNonNoneDirection = direction;
                lastNonNoneDistance = distance;
            }
        };

    }

    public static Direction ReverseDirectionX(Direction direction)
    {
        switch (direction)
        {
            case Swipeable.Direction.E:
                return Swipeable.Direction.W;
            case Swipeable.Direction.W:
                return Swipeable.Direction.E;
            case Swipeable.Direction.NE:
                return Swipeable.Direction.NW;
            case Swipeable.Direction.NW:
                return Swipeable.Direction.NE;
            case Swipeable.Direction.SW:
                return Swipeable.Direction.SE;
            case Swipeable.Direction.SE:
                return Swipeable.Direction.SW;
            default:
                return direction;
        }
    }
}
