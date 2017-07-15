using UnityEngine;

public static class ExtensionMethods {

	public static Texture2D ToTexture2D(this Sprite sprite)
	{
		if(sprite.rect.width != sprite.texture.width)
		{
			Texture2D texture = new Texture2D((int)sprite.rect.width,(int)sprite.rect.height, TextureFormat.RGBA32, false);
			Color[] newColors = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height );
			texture.SetPixels(newColors);
			texture.filterMode = FilterMode.Point;
			texture.Apply();
			return texture;
		}
		return sprite.texture;
	}
}
