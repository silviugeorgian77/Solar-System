using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    [SerializeField]
    private Transform sunTransform;

    [SerializeField]
    private float sunScale;

    [SerializeField]
    private float distanceScaleFactor;
    public float DistanceScaleFactor
    {
        get
        {
            return distanceScaleFactor;
        }
    }

    [SerializeField]
    private float moonExtraDistanceFactor;
    public float MoonExtraDistanceFactor
    {
        get
        {
            return moonExtraDistanceFactor;
        }
    }

    [SerializeField]
    private float diameterScaleFactor;
    public float DiameterScaleFactor
    {
        get
        {
            return diameterScaleFactor;
        }
    }

    [SerializeField]
    private float orbitTimeScaleFactor;
    public float OrbitTimeScaleFactor
    {
        get
        {
            return orbitTimeScaleFactor;
        }
    }

    private void Awake()
    {
        InitSunScale();
    }

    private void InitSunScale()
    {
        var scale = sunTransform.localScale;
        scale.x = sunScale;
        scale.y = sunScale;
        scale.z = sunScale;
        sunTransform.localScale = scale;
    }
}
