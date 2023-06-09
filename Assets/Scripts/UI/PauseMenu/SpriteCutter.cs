using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCutter : MonoBehaviour
{
    public Texture2D texture;

    private string savePath = "C:\\Users\\tm02246\\Documents\\GitHub\\Noctiluca\\Assets\\Scripts\\UI\\In-Game\\Characters";


    void Start()
    {
        //savePath = Application.dataPath;
        SaveTextures(SeperateTexture(texture));
    }

    // Seperate the images from the texture
    public Texture2D[] SeperateTexture(Texture2D texture) {
        //seperate texture by transparent gaps
        List<Texture2D> textures = new List<Texture2D>();
        Color[] pixels = texture.GetPixels();
        int width = texture.width;
        int height = texture.height;
        int x = 0;
        int y = 0;
        int i = 0;
        int j = 0;
        int k = 0;
        int l = 0;
        int m = 0;
        int n = 0;

        while (y < height) {
            while (x < width) {
                if (pixels[x + y * width].a != 0) {
                    i = x;
                    j = y;
                    k = x;
                    l = y;
                    m = x;
                    n = y;
                    while (pixels[i + j * width].a != 0) {
                        i++;
                    }
                    while (pixels[k + l * width].a != 0) {
                        l++;
                    }
                    while (pixels[m + n * width].a != 0) {
                        m++;
                    }
                    while (pixels[k + l * width].a != 0) {
                        n++;
                    }
                    textures.Add(CropTexture(texture, x, y, i - x, l - y));
                    x = i;
                }
                x++;
            }
            x = 0;
            y++;
        }
        return textures.ToArray();

    }

    // Crop the texture
    public Texture2D CropTexture(Texture2D texture, int x, int y, int width, int height) {
        Color[] pixels = texture.GetPixels();
        Color[] newPixels = new Color[width * height];
        int i = 0;
        int j = 0;
        int k = 0;
        int l = 0;
        while (j < height) {
            while (i < width) {
                newPixels[i + j * width] = pixels[(x + i) + (y + j) * texture.width];
                i++;
            }
            i = 0;
            j++;
        }
        Texture2D newTexture = new Texture2D(width, height);
        newTexture.SetPixels(newPixels);
        newTexture.Apply();
        return newTexture;
    }

    //Save Texture
    public void SaveTexture(Texture2D tex) {
        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(savePath + "/" + tex.name + ".png", bytes);
    }

    //Save Textures
    public void SaveTextures(Texture2D[] textures) {
        foreach (Texture2D texture in textures) {
            SaveTexture(texture);
        }
    }
}
