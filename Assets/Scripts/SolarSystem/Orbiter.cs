using UnityEngine;

public class Orbiter : MonoBehaviour
{
    [SerializeField]
    private Transform contentTransform;

    [SerializeField]
    private Transform pivotTransform;

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

    [SerializeField]
    private Rotatable rotatable;

    [SerializeField]
    private Follower follower;

    [SerializeField]
    private CircleRenderer circleRenderer;

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
        if (pivotTransform != null)
        {
            pivotRenderer = pivotTransform.GetComponent<SpriteRenderer>();
        }
        InitPivot();
        InitLookAtCamera();
        Init();
    }

    private void Init()
    {
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
                lastPivotSize = pivotSize;
                Init();
            }
        }
    }

    private void InitPivot()
    {
        follower.Target = pivotTransform;
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
}
