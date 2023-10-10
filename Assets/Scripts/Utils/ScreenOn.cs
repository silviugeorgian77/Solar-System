using UnityEngine;

public class ScreenOn : MonoBehaviour
{
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
