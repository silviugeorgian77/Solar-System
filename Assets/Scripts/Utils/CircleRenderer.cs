using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour
{
    [SerializeField]
    private int vertexCount = 40; // 4 vertices == square

    [SerializeField]
    private float lineWidth = 0.2f;

    [SerializeField]
    private float radius;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (vertexCount > 0 && lineWidth > 0 && radius > 0)
        {
            SetupCircle(vertexCount, lineWidth, radius);
        }
    }

    public void SetupCircle(
        int vertexCount = 40,
        float lineWidth = 0.2f,
        float radius = 1)
    {
        this.vertexCount = vertexCount;
        this.lineWidth = lineWidth;
        this.radius = radius;

        lineRenderer.positionCount = 0;

        lineRenderer.widthMultiplier = lineWidth;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        lineRenderer.positionCount = vertexCount;
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            pos = new Vector3(
                radius * Mathf.Cos(theta),
                radius * Mathf.Sin(theta),
                transform.position.z
            );
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
        lineRenderer.SetPosition(
            lineRenderer.positionCount - 1,
            lineRenderer.GetPosition(0)
        );
    }

//#if UNITY_EDITOR
//    private void OnDrawGizmos()
//    {
//        float deltaTheta = (2f * Mathf.PI) / vertexCount;
//        float theta = 0f;

//        Vector3 oldPos = Vector3.zero;
//        for (int i = 0; i < vertexCount + 1; i++)
//        {
//            var pos = new Vector3(
//                radius * Mathf.Cos(theta),
//                radius * Mathf.Sin(theta),
//                transform.position.z
//            );
//            Gizmos.DrawLine(oldPos, transform.position + pos);
//            oldPos = transform.position + pos;

//            theta += deltaTheta;
//        }
//    }
//#endif
}