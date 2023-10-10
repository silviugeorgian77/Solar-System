using UnityEngine;

public class TargetFrameRate : MonoBehaviour, SingletonEntry.Listener
{
    [SerializeField]
    private int targetFrameRate = 60;

    public void OnSingletonReady(bool isActiveSingleton)
    {
        if (isActiveSingleton)
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
