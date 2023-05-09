using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackgroundController : MonoBehaviour
{


    private RawImage rawImage;
    private RectTransform rectTransform;

    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    private Texture2D noiseTex;
    private Color[] pix;
    //private Renderer rend;


    [SerializeField] private float xSpeed = 0.5f;
    [SerializeField] private float ySpeed = 0.5f;


    float oldWidth;
    float oldHeight;
    void Start()
    {
        //rend = GetComponent<Renderer>();
        rawImage = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();

        // Set up the texture and a Color array to hold pixels during processing.
        InitilizeTexture();
    }

    void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                sample = Mathf.Round(sample);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    
    void InitilizeTexture() {
        pixWidth = (int)(rectTransform.rect.width/10);
        pixHeight = (int)(rectTransform.rect.height/10);
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];

        oldWidth = rectTransform.rect.width;
        oldHeight = rectTransform.rect.height;

        noiseTex.filterMode = FilterMode.Point;
        rawImage.material.mainTexture = noiseTex;
        rawImage.texture = noiseTex; 
    }

    void Update()
    {
        if(oldWidth != rectTransform.rect.width || oldHeight != rectTransform.rect.height) {
            InitilizeTexture();
        }
        

        CalcNoise();
        xOrg += xSpeed * Time.deltaTime;
        yOrg += ySpeed * Time.deltaTime;
    }

    
}
