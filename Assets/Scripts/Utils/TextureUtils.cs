using UnityEngine;

public class TextureUtils
{
    public static Texture2D GetTexture2D(string path)
    {
        byte[] bytes = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        return texture;
    }

    public static byte[] RenderTextureToBytes(RenderTexture renderTexture)
    {
        return RenderTextureToBytes(
            renderTexture,
            new Rect(0, 0, renderTexture.width, renderTexture.height)
        );
    }

    public static byte[] RenderTextureToBytes(
        RenderTexture renderTexture,
        Rect rect)
    {
        Texture2D texture = RenderTextureToTexture2D(renderTexture, rect);

        byte[] bytes = texture.EncodeToPNG();

        Object.Destroy(texture);

        return bytes;
    }

    public static Texture2D RenderTextureToTexture2D(
        RenderTexture renderTexture,
        Rect rect)
    {
        Texture2D texture = new Texture2D(
                (int)rect.width,
                (int)rect.height,
                TextureFormat.ARGB32,
                false,
                false
            );
        RenderTexture.active = renderTexture;
        texture.ReadPixels(
            rect,
            0,
            0
        );
        RenderTexture.active = null;
        texture.Apply();

        return texture;
    }

    public static Texture2D DuplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(
            new Rect(0, 0, renderTex.width, renderTex.height), 0, 0
        );
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    public static Texture2D RenderTextureToTexture2D(
        RenderTexture renderTexture)
    {
        Texture2D texture = new Texture2D(
                renderTexture.width,
                renderTexture.height,
                TextureFormat.ARGB32,
                false,
                false
            );
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = renderTexture;
        texture.ReadPixels(
            new Rect(0, 0, renderTexture.width, renderTexture.height),
            0,
            0
        );
        texture.Apply();
        return texture;
    }

    public static Texture2D GetTrimmedTexture(
        Texture2D texture,
        float minAlpha)
    {
        Rect trimmedTextureRect
            = GetTrimmedTextureRect(texture, minAlpha);
        Texture2D trimmedTexture = new Texture2D(
            (int)trimmedTextureRect.width,
            (int)trimmedTextureRect.height,
            TextureFormat.ARGB32,
            false,
            false
        );

        Color[] data = texture.GetPixels(
            (int)trimmedTextureRect.x,
            (int)trimmedTextureRect.y,
            (int)trimmedTextureRect.width,
            (int)trimmedTextureRect.height
        );
        trimmedTexture.SetPixels(data);
        trimmedTexture.Apply();

        return trimmedTexture;
    }

    public static Rect GetTrimmedTextureRect(
        Texture2D texture,
        float minAlpha)
    {
        int top = 0;
        int bottom = int.MaxValue;
        int right = 0;
        int left = int.MaxValue;
        Color[] pixels = texture.GetPixels();
        for (int y = 0; y < texture.height; ++y)
        {
            for (int x = 0; x < texture.width; ++x)
            {
                int index = GetPixelIndexInGetPixels(
                    x,
                    y,
                    texture.width,
                    texture.height
                );

                if (pixels[index].a >= minAlpha)
                {
                    if (y > top)
                    {
                        top = y;
                    }
                    if (y < bottom)
                    {
                        bottom = y;
                    }
                    if (x > right)
                    {
                        right = x;
                    }
                    if (x < left)
                    {
                        left = x;
                    }
                }
            }
        }
        return new Rect(left, bottom, right - left, top - bottom);
    }

    public static int GetPixelIndexInGetPixels(
        int x,
        int y,
        int xMax,
        int yMax)
    {
        if (x >= xMax || y >= yMax || x < 0 || y < 0)
        {
            return -1;
        }
        return ((y * xMax) + x);
    }
}
