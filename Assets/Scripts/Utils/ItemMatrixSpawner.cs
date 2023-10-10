using UnityEngine;
using System.Collections.Generic;

public class ItemMatrixSpawner : MonoBehaviour
{
    public enum SpawnDirectionX
    {
        LEFT_TO_RIGHT,
        RIGHT_TO_LEFT
    }

    public enum SpawnDirectionY
    {
        TOP_TO_BOTTOM,
        BOTTOM_TO_TOP
    }

    [SerializeField]
    private List<Transform> limitTransforms;

    [SerializeField]
    private GameObject itemPrefab;
    public GameObject ItemPrefab
    {
        get
        {
            return itemPrefab;
        }
        set
        {
            itemPrefab = value;
        }
    }

    [SerializeField]
    private Transform itemsParent;

    /// <summary>
    /// The reference of this aligner must be a GameObject that is not
    /// parented to this ItemMatrixSpawner.
    /// </summary>
    [SerializeField]
    private Aligner parentAligner;

    [SerializeField]
    private int itemCount = 30;

    [SerializeField]
    private float marginX = .3f;

    [SerializeField]
    private float marginY = .3f;

    [SerializeField]
    private int minRows;

    [SerializeField]
    private int minColumns;

    [SerializeField]
    private int maxRows = int.MaxValue;

    [SerializeField]
    private int maxColumns = int.MaxValue;

    [SerializeField]
    private SpawnDirectionX spawnDirectionX = SpawnDirectionX.LEFT_TO_RIGHT;

    [SerializeField]
    private SpawnDirectionY spawnDirectionY = SpawnDirectionY.TOP_TO_BOTTOM;

    [SerializeField]
    private bool resetOnAwake;

    public int Rows { get; private set; }
    public int Columns { get; private set; }
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private Vector2 spawnAreaSize;
    private float spawnAreaAspectRatio;

    public GameObject[,] SpawnedObjectsMatrix { get; private set; }
    public List<GameObject> SpawnedObjectsList { get; private set; }
        = new List<GameObject>();

    public delegate void OnItemMatrixChangedDelegate(
        ItemMatrixSpawner itemMatrixSpawner
    );
    public OnItemMatrixChangedDelegate OnItemMatrixChanged;

    private void Awake()
    {
        if (resetOnAwake)
        {
            ResetMatrix();
        }
    }

    public void ResetMatrix()
    {
        ClearSpawned();
        if (itemCount > 0)
        {
            Spawn();
        }
        OnItemMatrixChanged?.Invoke(this);
    }

    private void ClearSpawned()
    {
        if (SpawnedObjectsList != null)
        {
            foreach (GameObject spawnedObject in SpawnedObjectsList)
            {
                Destroy(spawnedObject);
            }
        }
        SpawnedObjectsMatrix = null;
        SpawnedObjectsList.Clear();
    }

    private void Spawn()
    {
        if (parentAligner != null)
        {
            parentAligner.checkEveryFrame = false;
            parentAligner.transform.localScale = Vector3.one;
        }
        InitLimits();
        ComputeSpawnAreaSize();
        PositionParentInCenterOfSpawnArea();
        ComputeSpawnAreaAspectRatio();
        ComputeTable();
        ComputeSpawnAreaSize();
        ComputeMargins();
        SpawnItems();
        PoitionItems();
        if (parentAligner != null)
        {
            parentAligner.Align();
            parentAligner.checkEveryFrame = true;
        }
    }

    public void InitLimits()
    {
        var limits = LimitUtils.GetLimits(
            LimitUtils.Mode.LOCAL,
            limitTransforms
        );
        minX = limits.w;
        maxX = limits.x;
        minY = limits.y;
        maxY = limits.z;
    }

    private void ComputeSpawnAreaSize()
    {
        spawnAreaSize = new Vector2();
        spawnAreaSize.x = maxX - minX;
        spawnAreaSize.y = maxY - minY;
    }

    private void PositionParentInCenterOfSpawnArea()
    {
        itemsParent.transform.localPosition = new Vector3(
            (minX + maxX) / 2f,
            (minY + maxY) / 2f,
            itemsParent.transform.localPosition.z
        );
    }

    private void ComputeSpawnAreaAspectRatio()
    {
        spawnAreaAspectRatio = spawnAreaSize.x / spawnAreaSize.y;
    }

    private void ComputeTable()
    {
        if (minColumns > 0 && minRows == 0)
        {
            ComputeTableByColumns();
        }
        else if (minRows > 0 && minColumns == 0)
        {
            ComputeTableByRows();
        }
        else
        {
            ComputeTableByColumns();
            Rows = (int)MathUtils.ClampValue(Rows, minRows, maxRows);
        }
    }

    private void ComputeTableByColumns()
    {
        Columns = (int)Mathf.Ceil(
            Mathf.Sqrt(spawnAreaAspectRatio * itemCount)
        );
        Columns = (int)MathUtils.ClampValue(Columns, minColumns, maxColumns);
        Rows = (int)Mathf.Ceil((float)itemCount / Columns);
    }

    private void ComputeTableByRows()
    {
        Rows = (int)Mathf.Ceil(
            Mathf.Sqrt(1 / spawnAreaAspectRatio * itemCount)
        );
        Rows = (int)MathUtils.ClampValue(Rows, minRows, maxRows);
        Columns = (int)Mathf.Ceil((float)itemCount / Rows);
    }

    private void ComputeMargins()
    {
        if (marginX == 0)
        {
            marginX = spawnAreaSize.x / Columns;
        }
        if (marginY == 0)
        {
            marginY = spawnAreaSize.y / Rows;
        }
    }

    private void SpawnItems()
    {
        GameObject spawnedObject;
        SpawnedObjectsMatrix = new GameObject[Rows, Columns];
        SpawnedObjectsList.Clear();
        int itemCount = 0;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (itemCount < this.itemCount)
                {
                    spawnedObject = Instantiate(itemPrefab, itemsParent);
                    SpawnedObjectsMatrix[i, j] = spawnedObject;
                    SpawnedObjectsList.Add(spawnedObject);
                    itemCount++;
                }
            }
        }
    }

    private void PoitionItems()
    {
        GameObject spawnedObject;
        float currentX;
        float currentY;
        if (spawnDirectionY == SpawnDirectionY.TOP_TO_BOTTOM)
        {
            currentY
                = Rows / 2f * marginY
                - .5f * marginY;
        }
        else
        {
            currentY
                = -Rows / 2f * marginY
                + .5f * marginY;
        }
        for (int i = 0; i < Rows; i++)
        {
            if (spawnDirectionX == SpawnDirectionX.LEFT_TO_RIGHT)
            {
                currentX
                    = -Columns / 2f * marginX
                    + .5f * marginX;
            }
            else
            {
                currentX
                    = Columns / 2f * marginX
                    -.5f * marginX;
            }

            for (int j = 0; j < Columns; j++)
            {
                spawnedObject = SpawnedObjectsMatrix[i, j];
                if (spawnedObject != null)
                {
                    spawnedObject.transform.localPosition = new Vector3(
                        currentX,
                        currentY,
                        spawnedObject.transform.localPosition.z
                    );
                }

                if (spawnDirectionX == SpawnDirectionX.LEFT_TO_RIGHT)
                {
                    currentX += + marginX;
                }
                else
                {
                    currentX -= + marginX;
                }
            }
            if (spawnDirectionY == SpawnDirectionY.TOP_TO_BOTTOM)
            {
                currentY -= marginY;
            }
            else
            {
                currentY += marginY;
            }
        }
    }
}
