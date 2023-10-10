using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    [SerializeField]
    private Transform sunTransform;

    /// <summary>
    /// In its natural scale, the sun is too big and the whole system is
    /// harder to navigate.
    /// Adjust this value in order to reduce the sun scale, but keep all
    /// the other planets scale.
    /// </summary>
    [SerializeField]
    private float sunScale;

    /// <summary>
    /// The distance between the planets and the sun
    /// (<see cref="Orbiter.distanceFromPivot"/>) will be multiplied by this
    /// value.
    /// </summary>
    [SerializeField]
    private float distanceScaleFactor;
    public float DistanceScaleFactor
    {
        get
        {
            return distanceScaleFactor;
        }
    }

    /// <summary>
    /// The diameter of all planets (<see cref="Orbiter.diameter"/>) will be
    /// multiplied by this value.
    /// </summary>
    [SerializeField]
    private float diameterScaleFactor;
    public float DiameterScaleFactor
    {
        get
        {
            return diameterScaleFactor;
        }
    }

    /// <summary>
    /// The orbit time of all planets (<see cref="Orbiter.orbitTime">) will be
    /// multiplied by this value.
    /// </summary>
    [SerializeField]
    private float orbitTimeScaleFactor;
    public float OrbitTimeScaleFactor
    {
        get
        {
            return orbitTimeScaleFactor;
        }
    }

    [SerializeField]
    private float orbitLineWidth;
    public float OrbitLineWidth
    {
        get
        {
            return orbitLineWidth;
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
