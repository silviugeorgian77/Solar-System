using UnityEngine;

public class Orbiter : MonoBehaviour
{
    [Header("Data")]
    /// <summary>
    /// The distance in from the pivot, measured in millions of kilometers.
    /// </summary>
    [SerializeField]
    private float distanceFromPivot;

    /// <summary>
    /// The diamter of the planet, measured in kilometers.
    /// </summary>
    [SerializeField]
    private float diameter;

    /// <summary>
    /// The orbit time, measured in days.
    /// </summary>
    [SerializeField]
    private float orbitTime;

    [Header("Internal")]
    /// <summary>
    /// Represents the parent transform of the renderers of the Orbiter.
    /// </summary>
    [SerializeField]
    private Transform contentTransform;

    /// <summary>
    /// The Orbiter will orbit around this transform.
    /// </summary>
    [SerializeField]
    private Transform pivotTargetTransform;

    [SerializeField]
    private Rotatable rotatable;

    /// <summary>
    /// This is used to follow <see cref="pivotTargetTransform"/> 
    /// </summary>
    [SerializeField]
    private Follower follower;

    [SerializeField]
    private CircleRenderer circleRenderer;

    /// <summary>
    /// This is used to always look at the camera.
    /// </summary>
    [SerializeField]
    private LookAtTransform lookAtTransform;

    private SolarSystem solarSystem;
    private float actualDistanceFromPivot;
    private float actualDiameter;
    private float actualOrbitTime;
    private SpriteRenderer pivotRenderer;
    private Vector3 pivotSize;
    private Vector3 lastPivotSize;

    private void Start()
    {
        solarSystem = Singleton.GetFirst<SolarSystem>();
        solarSystem.OnSystemSettingsChanged += OnSystemSettingsChanged;
        if (pivotTargetTransform != null)
        {
            pivotRenderer = pivotTargetTransform.GetComponent<SpriteRenderer>();
        }
        InitPivot();
        InitLookAtCamera();
        Init();
    }

    private void Init()
    {
        if (pivotRenderer != null)
        {
            pivotSize = pivotRenderer.bounds.size;
        }
        lastPivotSize = pivotSize;
        InitDistanceFromPivot();
        InitSize();
        InitOrbitTime();
        DrawOrbit();
    }

    private void Update()
    {
        if (pivotRenderer != null)
        {
            pivotSize = pivotRenderer.bounds.size;
            if (!pivotSize.AreApproximatelyEqual(lastPivotSize))
            {
                Init();
            }
        }
    }

    private void InitPivot()
    {
        follower.Target = pivotTargetTransform;
    }

    private void InitDistanceFromPivot()
    {
        actualDistanceFromPivot
            = distanceFromPivot * solarSystem.DistanceScaleFactor
            + pivotSize.x / 2f;

        var pos = contentTransform.localPosition;
        pos.x = actualDistanceFromPivot;
        contentTransform.localPosition = pos;
    }

    private void InitSize()
    {
        actualDiameter
            = diameter * solarSystem.DiameterScaleFactor;

        var scale = contentTransform.localScale;
        scale.x = actualDiameter;
        scale.y = actualDiameter;
        scale.z = actualDiameter;
        contentTransform.localScale = scale;
    }

    private void InitOrbitTime()
    {
        actualOrbitTime
            = orbitTime * solarSystem.OrbitTimeScaleFactor;

        if (actualOrbitTime <= 0)
        {
            return;
        }
        rotatable.RotateByZ(360, actualOrbitTime, EndCallBack: () =>
        {
            InitOrbitTime();
        });
    }

    private void InitLookAtCamera()
    {
        lookAtTransform.TargetTransform
            = Singleton.GetFirst<MainCamera>().transform;
    }

    private void DrawOrbit()
    {
        circleRenderer.SetupCircle(
            lineWidth: solarSystem.OrbitLineWidth,
            radius: actualDistanceFromPivot
        );
    }

    private void OnSystemSettingsChanged()
    {
        Init();
    }

    private void OnDestroy()
    {
        solarSystem.OnSystemSettingsChanged -= OnSystemSettingsChanged;
    }
}
