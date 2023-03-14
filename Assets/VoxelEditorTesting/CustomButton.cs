using UnityEngine;

public class CustomButton : MonoBehaviour
{
    public Texture2D backgroundTexture;
    public string buttonText;
    public GUIStyle textStyle;
    public float enlargeScale = 1.1f;
    public Color clickColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    
    private bool isClicked = false;
    private Rect buttonRect;
    private Rect textRect;

    private void OnGUI()
    {
        // Set button style
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.normal.background = backgroundTexture;

        // Calculate button size and position
        Vector2 buttonSize = buttonStyle.CalcSize(new GUIContent(""));
        buttonRect = new Rect((Screen.width - buttonSize.x) / 2, (Screen.height - buttonSize.y) / 2, buttonSize.x, buttonSize.y);

        // Calculate text size and position
        Vector2 textSize = textStyle.CalcSize(new GUIContent(buttonText));
        textRect = new Rect(buttonRect.x + (buttonRect.width - textSize.x) / 2, buttonRect.y + (buttonRect.height - textSize.y) / 2, textSize.x, textSize.y);

        // Draw button
        GUI.color = isClicked ? clickColor : Color.white;
        GUI.Button(buttonRect, "");

        // Draw text
        GUI.color = Color.white;
        GUI.Label(textRect, buttonText, textStyle);
    }

    private void OnMouseEnter()
    {
        // Enlarge button on hover
        buttonRect.x -= (buttonRect.width * enlargeScale - buttonRect.width) / 2;
        buttonRect.y -= (buttonRect.height * enlargeScale - buttonRect.height) / 2;
        buttonRect.width *= enlargeScale;
        buttonRect.height *= enlargeScale;
    }

    private void OnMouseExit()
    {
        // Reset button size on exit
        buttonRect.x += (buttonRect.width / enlargeScale - buttonRect.width) / 2;
        buttonRect.y += (buttonRect.height / enlargeScale - buttonRect.height) / 2;
        buttonRect.width /= enlargeScale;
        buttonRect.height /= enlargeScale;
    }

    private void OnMouseDown()
    {
        // Tint button on click
        isClicked = true;
    }

    private void OnMouseUp()
    {
        // Remove tint on release
        isClicked = false;
    }

    private void Start()
    {
        // Set text style
        textStyle = new GUIStyle(GUI.skin.label);
        textStyle.fontSize = 20;
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.normal.textColor = Color.white;
        textStyle.normal.background = MakeTex(2, 2, Color.black);
    }

    // Helper function to create a texture
    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = color;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
