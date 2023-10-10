using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class CollidersEnabler : MonoBehaviour
{
    [SerializeField]
    private List<Collider> colliders = new List<Collider>();
    public List<Collider> Colliders
    {
        get
        {
            return colliders;
        }
    }

    [SerializeField]
    private bool isEnabled;
    public bool IsEnabled
    {
        get
        {
            return isEnabled;
        }
        set
        {
            isEnabled = value;
            SetCollidersEnabled(isEnabled);
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
            SetCollidersEnabled(isEnabled);
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
        colliders.Clear();
        colliders.AddRange(
            GetComponentsInChildren<Collider>()
        );
        if (lookInParents)
        {
            var tempColliders = GetComponentsInParent<Collider>();
            foreach (var tempCollider in tempColliders)
            {
                if (!colliders.Contains(tempCollider))
                {
                    colliders.Add(tempCollider);
                }
            }
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }


    public void SetCollidersEnabled(bool isEnabled)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = isEnabled;
        }
    }
}
