using UnityEngine;

public static class CameraUtils
{
    public static Vector2 GetOrtographicSize(this Camera camera)
    {
        Vector2 size = new Vector2();
        size.y = 2 * camera.orthographicSize;
        size.x = size.y * camera.aspect;
        return size;
    }

    public static Vector2 GetPerspectiveFrustumSize(this Camera camera)
    {
        float distance = Mathf.Abs(camera.transform.position.z);
        return GetPerspectiveFrustumSize(camera, distance);
    }

    public static Vector2 GetPerspectiveFrustumSize(
        Camera camera,
        float distance)
    {
        Vector2 size = new Vector2();
        size.y 
            = 2.0f
            * distance 
            * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        size.x = size.y * camera.aspect;
        return size;
    }

    public static float GetOrtographicCameraAspectRatio(this Camera camera)
    {
        Vector2 cameraSize = GetOrtographicSize(camera);
        return cameraSize.x / cameraSize.y;
    }

    public enum CameraMatcherReferenceSize
    {
        WIDTH,
        HEIGHT,
        NONE
    }

    public static void MatchTransform(
        this Camera camera,
        Transform transform,
        CameraMatcherReferenceSize cameraMatcherReferenceSize,
        Camera referenceCamera)
    {
        Vector2 viewportSize = MathUtils.GetSizeOfTransform(
            transform
        );

        Vector2 bottomLeftPoint = new Vector2(
            transform.position.x - viewportSize.x / 2,
            transform.position.y - viewportSize.y / 2
        );

        Vector2 topRightPoint = new Vector2(
            transform.position.x + viewportSize.x / 2,
            transform.position.y + viewportSize.y / 2
        );

        Vector3 transformedBottomLeftPoint
            = referenceCamera.WorldToViewportPoint(
                bottomLeftPoint
            );

        Vector3 transformedTopRightPoint
            = referenceCamera.WorldToViewportPoint(
                topRightPoint
            );

        float transformedWidth
            = transformedTopRightPoint.x
            - transformedBottomLeftPoint.x;

        float transformedHeight
            = transformedTopRightPoint.y
            - transformedBottomLeftPoint.y;

        camera.rect = new Rect(
            transformedBottomLeftPoint.x,
            transformedBottomLeftPoint.y,
            transformedWidth,
            transformedHeight
        );

        camera.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            camera.transform.position.z
        );

        if (cameraMatcherReferenceSize == CameraMatcherReferenceSize.HEIGHT)
        {
            camera.orthographicSize
                = referenceCamera.orthographicSize
                * transformedHeight;
        }
        else if (cameraMatcherReferenceSize == CameraMatcherReferenceSize.WIDTH)
        {
            camera.orthographicSize
                = referenceCamera.orthographicSize
                * transformedWidth;
        }
    }

    public static bool HasNotch()
    {
        return Screen.safeArea.yMax < Screen.height;
    }

    public static void SetLayerMaskEnabled(
        this Camera camera,
        string layerName,
        bool enabled)
    {
        if (enabled)
        {
            camera.cullingMask |= 1 << LayerMask.NameToLayer(layerName);
        }
        else
        {
            camera.cullingMask &= ~(1 << LayerMask.NameToLayer(layerName));
        }
    }
}
