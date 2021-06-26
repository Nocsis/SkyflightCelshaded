using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshRenderer))]
public class Paintable : MonoBehaviour
{
    private int penSize = 10;
    private Color[] color;

    private Texture2D startTexture;
    private Texture2D texture;
    private Texture2D newTexture;
    private Color32[] col_originalTexture;
    private Color32[] col_currentTexture;

    private int textureWidth;
    private int textureHeight;

    private bool touching, touchingLast;
    private float posX, posY;
    private float lastX, lastY;

    private void Awake()
    {
        texture = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        textureWidth = texture.width;
        textureHeight = texture.height;

        startTexture = texture; //TODO: Prüfen ob startTexture gebraucht wird, oder ob col_originalTexture reicht und newTexture aus texture erstellt werden kann
        col_originalTexture = texture.GetPixels32();

        newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false, true);
        newTexture.SetPixels32(startTexture.GetPixels32());
        newTexture.Apply();

        col_currentTexture = new Color32[textureWidth * textureHeight];
        newTexture.GetPixels32().CopyTo(col_currentTexture, 0);

        GetComponent<MeshRenderer>().material.mainTexture = newTexture;
    }

    void Update()
    {
        // Transform textureCoords into "pixel" values
        int x = (int)(posX * textureWidth - (penSize / 2));
        int y = (int)(posY * textureHeight - (penSize / 2));

        // Only set the pixels if we were touching last frame
        if (touchingLast)
        {
            // Set base touch pixels
            newTexture.SetPixels(x, y, penSize, penSize, color);

            // Interpolate pixels from previous touch
            for (float t = 0.01f; t < 1.00f; t += 0.01f)
            {
                int lerpX = (int)Mathf.Lerp(lastX, (float)x, t);
                int lerpY = (int)Mathf.Lerp(lastY, (float)y, t);
                newTexture.SetPixels(lerpX, lerpY, penSize, penSize, color);
            }
        }

        // If currently touching, apply the texture
        if (touching)
        {
            newTexture.Apply();
        }

        this.lastX = (float)x;
        this.lastY = (float)y;

        this.touchingLast = this.touching;
    }

    public void ToggleTouch(bool touching)
    {
        this.touching = touching;
    }

    public void SetTouchPosition(float x, float y)
    {
        this.posX = x;
        this.posY = y;
    }

    public void SetColor(Color color)
    {
        this.color = Enumerable.Repeat<Color>(color, penSize * penSize).ToArray<Color>();
    }
}
