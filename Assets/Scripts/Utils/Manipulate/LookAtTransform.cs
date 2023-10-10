using UnityEngine;

public class LookAtTransform : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;
    public Transform TargetTransform
    {
        get
        {
            return targetTransform;
        }
        set
        {
            targetTransform = value;
        }
    }

    private void Update()
    {
        UpdateLook();
    }

    public void UpdateLook()
    {
        if (targetTransform != null)
        {
            transform.LookAt(targetTransform);
        }
    }
}
