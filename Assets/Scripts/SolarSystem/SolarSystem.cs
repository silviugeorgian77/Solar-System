using UnityEngine;

public class SolarSystem : MonoBehaviour
{
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

    [SerializeField]
    private float orbitLineWidth;
    public float OrbitLineWidth
    {
        get
        {
            return orbitLineWidth;
        }
    }
}
