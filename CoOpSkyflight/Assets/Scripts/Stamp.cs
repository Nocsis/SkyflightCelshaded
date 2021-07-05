using UnityEngine;

public enum PaintMode : byte
{
    Draw,
    Erase
}

public class Stamp
{
    private float[] Pixels;
    public float[] CurrentPixels;

    public int Width;
    public int Height;

    public PaintMode mode = PaintMode.Draw;

    public Stamp(Texture2D stampTexture)
    {
        Width = stampTexture.width;
        Height = stampTexture.height;

        Pixels = new float[Width * Height];
        CurrentPixels = new float[Width * Height];

        float alphaValue;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                alphaValue = stampTexture.GetPixel(x, y).a;
                Pixels[x + y * Width] = alphaValue;
                CurrentPixels[x + y * Width] = alphaValue;
            }
        }
    }
}
