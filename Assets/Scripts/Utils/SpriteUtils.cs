using UnityEngine;

public class SpriteUtils
{
    public static Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            Texture2D texture = TextureUtils.GetTexture2D(path);
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Sprite sprite = Sprite.Create(
                texture,
                rect,
                new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }

    public static Sprite LoadTrimmedSprite(string path, float minAlpha)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            Texture2D texture = TextureUtils.GetTexture2D(path);
            Rect rect = TextureUtils.GetTrimmedTextureRect(texture, minAlpha);
            Sprite sprite = Sprite.Create(
                texture,
                rect,
                new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }
}
