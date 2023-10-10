using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private SolarSystem solarSystem;

    [SerializeField]
    private Slider sunScaleSlider;

    [SerializeField]
    private Slider distanceScaleSlider;

    [SerializeField]
    private Slider diameterScaleSlider;

    [SerializeField]
    private Slider orbitTimeScaleSlider;

    [SerializeField]
    private GameObject readMeObject;

    private void Awake()
    {
        sunScaleSlider.value = solarSystem.SunScale;
        distanceScaleSlider.value = solarSystem.DistanceScaleFactor;
        diameterScaleSlider.value = solarSystem.DiameterScaleFactor;
        orbitTimeScaleSlider.value = solarSystem.OrbitTimeScaleFactor;

        sunScaleSlider.onValueChanged.AddListener(OnSunScaleChanged);
        distanceScaleSlider.onValueChanged.AddListener(OnDistanceScaleChanged);
        diameterScaleSlider.onValueChanged.AddListener(OnDiameterScaleChanged);
        orbitTimeScaleSlider.onValueChanged.AddListener(OnOrbitTimeScaleChanged);
    }

    private void OnSunScaleChanged(float value)
    {
        solarSystem.SunScale = value;
    }

    private void OnDistanceScaleChanged(float value)
    {
        solarSystem.DistanceScaleFactor = value;
    }

    private void OnDiameterScaleChanged(float value)
    {
        solarSystem.DiameterScaleFactor = value;
    }

    private void OnOrbitTimeScaleChanged(float value)
    {
        solarSystem.OrbitTimeScaleFactor = value;
    }

    public void OnReadMeClicked()
    {
        readMeObject.SetActive(!readMeObject.activeSelf);
    }

    private void OnDestroy()
    {
        sunScaleSlider.onValueChanged.RemoveListener(OnSunScaleChanged);
        distanceScaleSlider.onValueChanged.RemoveListener(OnDistanceScaleChanged);
        diameterScaleSlider.onValueChanged.RemoveListener(OnDiameterScaleChanged);
        orbitTimeScaleSlider.onValueChanged.RemoveListener(OnOrbitTimeScaleChanged);
    }
}
