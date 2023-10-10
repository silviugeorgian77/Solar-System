using UnityEngine;

public class GravityScale : MonoBehaviour
{
    [SerializeField]
    private float scale = 1f;
    public float Scale
    {
        get
        {
            return scale;
        }
        set
        {
            scale = value;
        }
    }

    [SerializeField]
    private Rigidbody rigidBody;
    public Rigidbody RigidBody
    {
        get
        {
            return rigidBody;
        }
        set
        {
            rigidBody = value;
            rigidBody.useGravity = false;
        }
    }

    [SerializeField]
    private bool autoAssignReferences;

    private void Start()
    {
        if (autoAssignReferences)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
        rigidBody.useGravity = false;
    }

    private void FixedUpdate()
    {
        // It has to be FixedUpdate, because it applies force to the
        // rigidbody constantly.
        rigidBody.AddForce(
            Physics.gravity * scale,
            ForceMode.Acceleration
        );
    }
}