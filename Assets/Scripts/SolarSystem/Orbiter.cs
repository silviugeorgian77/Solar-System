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
    private SpriteRenderer pivotTargetRenderer;
    private Vector3 pivotSize;
    private Vector3 lastPivotSize;

    private void Start()
    {
        solarSystem = Singleton.GetFirst<SolarSystem>();
        solarSystem.OnSystemSettingsChanged += OnSystemSettingsChanged;
        if (pivotTargetTransform != null)
        {
            pivotTargetRenderer
                = pivotTargetTransform.GetComponent<SpriteRenderer>();
        }
        InitPivot();
        InitLookAtCamera();
        InitOrbitBehaviour();
    }

    /// <summary>
    /// Pulls the settings from the <see cref="SolarSystem"/> and initializes
    /// the orbit routine.
    /// </summary>
    private void InitOrbitBehaviour()
    {
        if (pivotTargetRenderer != null)
        {
            pivotSize = pivotTargetRenderer.bounds.size;
        }
        lastPivotSize = pivotSize;
        InitDistanceFromPivot();
        InitSize();
        InitOrbitTime();
        DrawOrbit();
    }

    private void Update()
    {
        ManagePivotTargetSize();
    }

    /// <summary>
    /// If the pivot size changes, then we must recompute the orbit routine.
    /// 
    /// Because the pivot can also be an <see cref="Orbiter"/>
    /// (the Moon for the Earth), we have the scenario where pivot must first
    /// finish its scaling via <see cref="InitSize"/>.
    ///
    /// Since <see cref="InitSize"/> is firstly called  in <see cref="Start"/>
    /// for both the pivot target and the pivot, it's just safer to check
    /// in the update loop for size changes of the pivot target.
    ///
    /// If there is a size change, then we need to reinitialize.
    /// </summary>
    private void ManagePivotTargetSize()
    {
        if (pivotTargetRenderer != null)
        {
            pivotSize = pivotTargetRenderer.bounds.size;
            if (!pivotSize.AreApproximatelyEqual(lastPivotSize))
            {
                InitOrbitBehaviour();
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

    /// <summary>
    /// Draws the circle outline of the orbit.
    /// </summary>
    private void DrawOrbit()
    {
        circleRenderer.SetupCircle(
            lineWidth: solarSystem.OrbitLineWidth,
            radius: actualDistanceFromPivot
        );
    }

    private void OnSystemSettingsChanged()
    {
        InitOrbitBehaviour();
    }

    private void OnDestroy()
    {
        solarSystem.OnSystemSettingsChanged -= OnSystemSettingsChanged;
    }
}
