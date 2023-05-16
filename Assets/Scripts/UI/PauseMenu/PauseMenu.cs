using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Texture2D cursorDefaultTexture;
    public Texture2D cursorHoverTexture;
    public Texture2D cursorClickTexture;

    // Start is called before the first frame update
    void Start()
    {
        //set cursor image
        Cursor.SetCursor(cursorDefaultTexture, Vector2.zero, CursorMode.Auto);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
