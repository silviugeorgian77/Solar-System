using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PhysicsEnabler : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    public List<Rigidbody> Rigidbodies
    {
        get
        {
            return rigidbodies;
        }
    }

    [SerializeField]
    private bool isKinematic;
    public bool IsKinematic
    {
        get
        {
            return isKinematic;
        }
        set
        {
            isKinematic = value;
            SetIsKinematic(isKinematic);
        }
    }

    [SerializeField]
    private bool detectCollisions;
    public bool DetectCollisions
    {
        get
        {
            return detectCollisions;
        }
        set
        {
            detectCollisions = value;
            SetDetectCollisions(detectCollisions);
        }
    }

    [SerializeField]
    private bool executeOnAwake;

    [SerializeField]
    private bool lookInParents;

    [SerializeField]
    private bool autoFetchComponents;

    private void Awake()
    {
        ManageAutoFetch();
        if (executeOnAwake)
        {
            SetIsKinematic(isKinematic);
            SetDetectCollisions(detectCollisions);
        }
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        EditorApplication.update += Update;
    }

    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    private void Update()
    {
        ManageAutoFetch();
    }
#endif

    private void ManageAutoFetch()
    {
        if (autoFetchComponents)
        {
            FetchComponents();
            autoFetchComponents = false;
        }
    }

    public void FetchComponents()
    {
        rigidbodies.Clear();
        rigidbodies.AddRange(
            GetComponentsInChildren<Rigidbody>()
        );
        if (lookInParents)
        {
            var tempRigidBody = GetComponentsInParent<Rigidbody>();
            foreach (var tempNetworkBehviour in tempRigidBody)
            {
                if (!rigidbodies.Contains(tempNetworkBehviour))
                {
                    rigidbodies.Add(tempNetworkBehviour);
                }
            }
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public void SetPhysicsEnabled(bool physicsEnabled)
    {
        SetIsKinematic(!physicsEnabled);
        SetDetectCollisions(physicsEnabled);
    }

    public void SetIsKinematic(bool isKinematic)
    {
        foreach (var rigidBody in rigidbodies)
        {
            rigidBody.isKinematic = isKinematic;
        }
    }

    public void SetDetectCollisions(bool detectCollisions)
    {
        foreach (var rigidBody in rigidbodies)
        {
            rigidBody.detectCollisions = detectCollisions;
        }
    }

    public void SetConstraintsToFreezeAll()
    {
        foreach (var rigidBody in rigidbodies)
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void SetConstraintsToNone()
    {
        foreach (var rigidBody in rigidbodies)
        {
            rigidBody.constraints = RigidbodyConstraints.None;
        }
    }
}
