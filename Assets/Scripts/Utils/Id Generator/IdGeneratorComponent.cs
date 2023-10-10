using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class IdGeneratorComponent : MonoBehaviour
{
    [SerializeField]
    private long id;

    public long Id {
        get {
            GenerateIdIfNecessary();
            return id;
        }
    }

    private void Awake()
    {
        GenerateIdIfNecessary();
    }

    private void Update()
    {
        GenerateIdIfNecessary();
    }

    private void GenerateIdIfNecessary()
    {
        if (id <= 0)
        {
            GenerateId();
        }
    }

    private void GenerateId()
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "Id Change");
#endif
        id = IdGenerator.GenerateUniqueId();
    }
}
