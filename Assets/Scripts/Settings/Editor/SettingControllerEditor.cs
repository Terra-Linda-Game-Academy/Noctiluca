using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SettingController))]
public class SettingControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        int selected = 0;
        string[] options = new string[]
        {
     "Option1", "Option2", "Option3",
        };
        selected = EditorGUILayout.Popup("Label", selected, options);
        serializedObject.Update();
        //EditorGUILayout.PropertyField(lookAtPoint);
        serializedObject.ApplyModifiedProperties();
    }
}
