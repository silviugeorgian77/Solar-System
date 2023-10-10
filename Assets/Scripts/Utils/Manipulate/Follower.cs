using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    public Transform Target
    {
        get { return target; }
        set
        {
            target = value;
            ComputePosOffset();
            ComputeAngleOffset();
        }
    }

    [SerializeField]
    private float smoothingPos = 5f;
    public float SmoothingPos
    {
        get
        {
            return smoothingPos;
        }
        set
        {
            smoothingPos = value;
        }
    }

    [SerializeField]
    private bool enabledPosX = true;
    public bool EnabledPosX
    {
        get
        {
            return enabledPosX;
        }
        set
        {
            enabledPosX = value;
        }
    }

    [SerializeField]
    private bool enabledPosY = true;
    public bool EnabledPosY
    {
        get
        {
            return enabledPosY;
        }
        set
        {
            enabledPosY = value;
        }
    }

    [SerializeField]
    private bool enabledPosZ = true;
    public bool EnabledPosZ
    {
        get
        {
            return enabledPosZ;
        }
        set
        {
            enabledPosZ = value;
        }
    }

    [SerializeField]
    private float minPosX = float.MinValue;
    public float MinPosX
    {
        get
        {
            return minPosX;
        }
        set
        {
            minPosX = value;
        }
    }

    [SerializeField]
    private float maxPosX = float.MaxValue;
    public float MaxPosX
    {
        get
        {
            return maxPosX;
        }
        set
        {
            maxPosX = value;
        }
    }

    [SerializeField]
    private float minPosY = float.MinValue;
    public float MinPosY
    {
        get
        {
            return minPosY;
        }
        set
        {
            minPosY = value;
        }
    }

    [SerializeField]
    private float maxPosY = float.MaxValue;
    public float MaxPosY
    {
        get
        {
            return maxPosY;
        }
        set
        {
            maxPosY = value;
        }
    }

    [SerializeField]
    private float minPosZ = float.MinValue;
    public float MinPosZ
    {
        get
        {
            return minPosZ;
        }
        set
        {
            minPosZ = value;
        }
    }

    [SerializeField]
    private float maxPosZ = float.MaxValue;
    public float MaxPosZ
    {
        get
        {
            return maxPosZ;
        }
        set
        {
            maxPosZ = value;
        }
    }

    [SerializeField]
    private bool supportInitPosOffset;

    [SerializeField]
    private float smoothingAngle = 5f;
    public float SmoothingAngle
    {
        get
        {
            return smoothingAngle;
        }
        set
        {
            smoothingAngle = value;
        }
    }

    [SerializeField]
    private bool enabledAngleX = true;
    public bool EnabledAngleX
    {
        get
        {
            return enabledAngleX;
        }
        set
        {
            enabledAngleX = value;
        }
    }

    [SerializeField]
    private bool enabledAngleY = true;
    public bool EnabledAngleY
    {
        get
        {
            return enabledAngleY;
        }
        set
        {
            enabledAngleY = value;
        }
    }

    [SerializeField]
    private bool enabledAngleZ = true;
    public bool EnabledAngleZ
    {
        get
        {
            return enabledAngleZ;
        }
        set
        {
            enabledAngleZ = value;
        }
    }

    [SerializeField]
    private float minAngleX = float.MinValue;
    public float MinAngleX
    {
        get
        {
            return minAngleX;
        }
        set
        {
            minAngleX = value;
        }
    }

    [SerializeField]
    private float maxAngleX = float.MaxValue;
    public float MaxAngleX
    {
        get
        {
            return maxAngleX;
        }
        set
        {
            maxAngleX = value;
        }
    }

    [SerializeField]
    private float minAngleY = float.MinValue;
    public float MinAngleY
    {
        get
        {
            return minAngleY;
        }
        set
        {
            minAngleY = value;
        }
    }

    [SerializeField]
    private float maxAngleY = float.MaxValue;
    public float MaxAngleY
    {
        get
        {
            return maxAngleY;
        }
        set
        {
            maxAngleY = value;
        }
    }

    [SerializeField]
    private float minAngleZ = float.MinValue;
    public float MinAngleZ
    {
        get
        {
            return minAngleZ;
        }
        set
        {
            minAngleZ = value;
        }
    }

    [SerializeField]
    private float maxAngleZ = float.MaxValue;
    public float MaxAngleZ
    {
        get
        {
            return maxAngleZ;
        }
        set
        {
            maxAngleZ = value;
        }
    }

    [SerializeField]
    private bool supportInitAngleOffset;

    private Vector3 posOffset = Vector3.zero;
    private Vector3 angleOffset = Vector3.zero;

    private void Awake()
    {
        ComputePosOffset();
        ComputeAngleOffset();
    }

    private void FixedUpdate()
    {
        UpdatePos(smoothingPos);
        UpdateAngle(smoothingAngle);
    }

    public void UpdatePos(float smoothingPos)
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPos = target.position + posOffset;

        if (!enabledPosX)
        {
            targetPos.x = transform.position.x;
        }
        if (!enabledPosY)
        {
            targetPos.y = transform.position.y;
        }
        if (!enabledPosZ)
        {
            targetPos.z = transform.position.z;
        }

        if (smoothingPos != 0)
        {
            targetPos = Vector3.Lerp(
                transform.position,
                targetPos,
                smoothingPos * Time.deltaTime
            );
        }

        if (enabledPosX)
        {
            targetPos.x = MathUtils.ClampValue(
                targetPos.x,
                minPosX,
                maxPosX
            );
        }
        if (enabledPosY)
        {
            targetPos.y = MathUtils.ClampValue(
                targetPos.y,
                minPosY,
                maxPosY
            );
        }
        if (enabledPosZ)
        {
            targetPos.z = MathUtils.ClampValue(
                targetPos.z,
                minPosZ,
                maxPosZ
            );
        }

        if (enabledPosX || enabledPosY || enabledPosZ)
        {
            transform.position = targetPos;
        }
    }

    public void UpdateAngle(float smoothingAngle)
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetAngle = target.eulerAngles + angleOffset;

        if (!enabledAngleX)
        {
            targetAngle.x = transform.eulerAngles.x;
        }
        if (!enabledAngleY)
        {
            targetAngle.y = transform.eulerAngles.y;
        }
        if (!enabledAngleZ)
        {
            targetAngle.z = transform.eulerAngles.z;
        }

        if (smoothingAngle != 0)
        {
            targetAngle = Vector3.Lerp(
                transform.eulerAngles,
                targetAngle,
                smoothingAngle * Time.deltaTime
            );
        }

        if (enabledAngleX)
        {
            targetAngle.x = MathUtils.ClampValue(
                targetAngle.x,
                minAngleX,
                maxAngleX
            );
        }
        if (enabledAngleY)
        {
            targetAngle.y = MathUtils.ClampValue(
                targetAngle.y,
                minAngleY,
                maxAngleY
            );
        }
        if (enabledAngleZ)
        {
            targetAngle.z = MathUtils.ClampValue(
                targetAngle.z,
                minAngleZ,
                maxAngleZ
            );
        }

        if (enabledAngleX || enabledAngleY || enabledAngleZ)
        {
            transform.eulerAngles = targetAngle;
        }
    }

    public void ComputePosOffset()
    {
        if (supportInitPosOffset && target != null)
        {
            posOffset = transform.position - target.position;
        }
        else
        {
            posOffset = Vector3.zero;
        }
    }

    public void ComputeAngleOffset()
    {
        if (supportInitAngleOffset && target != null)
        {
            angleOffset = transform.eulerAngles - target.eulerAngles;
        }
        else
        {
            angleOffset = Vector3.zero;
        }
    }
}
