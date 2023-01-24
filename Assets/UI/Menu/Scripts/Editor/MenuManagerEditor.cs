using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MenuManager))]
public class MenuManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MenuManager menuManagerScript = (MenuManager)target;
        if(GUILayout.Button("Load Sections"))
        {
            menuManagerScript.LoadSections();
        }

        DrawDefaultInspector();
    }
}
