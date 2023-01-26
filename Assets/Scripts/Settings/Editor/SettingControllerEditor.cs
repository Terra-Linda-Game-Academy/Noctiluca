using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.Events;

[CustomEditor(typeof(SettingController), true)]
public class SettingControllerEditor : Editor
{
    int selected = 0;
    int selected2 = 0;

    public override void OnInspectorGUI()
    {
        SettingController selfController = (SettingController)target;

        EditorGUILayout.LabelField("Selected Variable", EditorStyles.boldLabel);
        //category
        Dictionary<string, SettingsCategory> settingsCategories = new Dictionary<string, SettingsCategory>();
        foreach(FieldInfo fieldInfo in typeof(Settings).GetFields()) {
            if(typeof(SettingsCategory).IsAssignableFrom(fieldInfo.FieldType)) {
                settingsCategories.Add(fieldInfo.Name, (SettingsCategory)fieldInfo.GetValue(null));
            }
        }
        
        string[] options = new string[settingsCategories.Keys.Count];
        settingsCategories.Keys.CopyTo(options, 0);

        selected = EditorGUILayout.Popup("Selected Catagory", selected, options);

        //category settings
        Dictionary<string, FieldInfo> settingsVariables = new Dictionary<string, FieldInfo>();
        foreach(FieldInfo fieldInfo in settingsCategories[options[selected]].GetType().GetFields()) {
            if(fieldInfo.FieldType == typeof(SettingVariable))
                settingsVariables.Add(fieldInfo.Name, fieldInfo);
        }
        
        string[] options2 = new string[settingsVariables.Keys.Count];
        settingsVariables.Keys.CopyTo(options2, 0);

        
        
        selected2 = EditorGUILayout.Popup("Selected Setting", selected2, options2);
        EditorGUILayout.LabelField(options[selected]+"."+options2[selected2], EditorStyles.helpBox);
        EditorGUILayout.Separator();
        
        

       selfController.settingVariable = (SettingVariable)settingsVariables[options2[selected2]].GetValue(settingsCategories[options[selected]]);
       


        serializedObject.Update();
        //EditorGUILayout.PropertyField(lookAtPoint);
        serializedObject.ApplyModifiedProperties();
    }
}
