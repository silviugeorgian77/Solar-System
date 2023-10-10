using UnityEngine;
using System.IO;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScreenshotHandler : MonoBehaviour
{
    public int screenshotScale = 1;
    private Camera screenshotCamera;

    private void Awake()
    {
        screenshotCamera = GetComponent<Camera>();
    }

    public void TakeScreenShot(string directoryPath, string fileName)
    {
        Vector2 cameraSize = CameraUtils.GetOrtographicSize(screenshotCamera);

		int resWidthN = (int)(cameraSize.x * screenshotScale);
		int resHeightN = (int)(cameraSize.y * screenshotScale);
		RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);
		screenshotCamera.targetTexture = rt;

		Texture2D screenShot = new Texture2D(
            resWidthN, resHeightN, TextureFormat.ARGB32, false
            );
		screenshotCamera.Render();
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
		screenshotCamera.targetTexture = null;
		RenderTexture.active = null;
		byte[] bytes = screenShot.EncodeToPNG();

        DirectoryInfo dir = new DirectoryInfo(directoryPath);
        dir.Create();
        string fullPath = directoryPath
            + Path.DirectorySeparatorChar
            + fileName;
        File.WriteAllBytes(fullPath, bytes);
	}
}
