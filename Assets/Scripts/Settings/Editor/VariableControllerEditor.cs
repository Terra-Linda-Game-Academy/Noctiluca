using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.Events;
using System;

[CustomEditor(typeof(VariableController), true)]
public class VariableControllerEditor : Editor
{

    private static int[] selected = new int[10];
    private int m_LayerLength;

    public int LayerLength
    {
        get { return m_LayerLength; }
        set { 
            m_LayerLength = value;
            if (m_LayerLength > 50)
            {
                m_LayerLength = 50;
            }
            else if (m_LayerLength < 1)
            {
                m_LayerLength = 1;
            }
            selected = new int[m_LayerLength];
        }
    }

    
    public override void OnInspectorGUI()
    {


        DrawDefaultInspector();

        LayerLength = EditorGUILayout.IntField("Reflection Distance", LayerLength);

        VariableController selfController = (VariableController)target;
        bool stillMoreLayers = true;
        Type currentType;
        object instance = selfController.script;
        if (selfController.script == null)
        {
            currentType = typeof(Settings);
        } else
        {
            currentType = selfController.script.GetType();
        }

        int layer = 0;
         
        string directory = "";
        List<FieldInfo> layers = new List<FieldInfo>();
        while (stillMoreLayers && currentType.GetFields().Length > 0 && layer < selected.Length)
        {
            //EditorGUILayout.LabelField("Selected Variable", EditorStyles.boldLabel);
            //category
           // Debug.Log("Type: " + currentType);
            Dictionary<string, FieldInfo> settingVariables = new Dictionary<string, FieldInfo>();
            foreach (FieldInfo fieldInfo in currentType.GetFields())
            {
                settingVariables.Add(fieldInfo.Name, fieldInfo);
            }

            string[] options = new string[settingVariables.Keys.Count];
            settingVariables.Keys.CopyTo(options, 0);
            selected[layer] = EditorGUILayout.Popup("Selected Category", selected[layer], options);

            Type fieldType = settingVariables[options[selected[layer]]].FieldType;
            FieldInfo field = settingVariables[options[selected[layer]]];

            layers.Add(field);
            

            directory += field.Name;
            if (fieldType.GetFields().Length > 0 && fieldType != typeof(SettingVariable))
            {
                //Continue
                stillMoreLayers = true;
                currentType = fieldType;
                layer++;
                directory += ".";
            }
             else
            {
                stillMoreLayers = false;
                if(fieldType  == typeof(SettingVariable))
                {
                    object previousLayerObject = instance;
                    foreach(FieldInfo fieldInfo in layers)
                    {
                        previousLayerObject = fieldInfo.GetValue(previousLayerObject);
                    }
                    selfController.Variable = previousLayerObject;
                }
            }
        }

        EditorGUILayout.LabelField(directory, EditorStyles.helpBox);
        EditorGUILayout.Separator();

        serializedObject.Update();
        //EditorGUILayout.PropertyField(lookAtPoint);
        serializedObject.ApplyModifiedProperties();


        /*
        //category settings
        Dictionary<string, FieldInfo> settingsVariables = new Dictionary<string, FieldInfo>();
        foreach (FieldInfo fieldInfo in settingsCategories[options[selected]].GetType().GetFields())
        {
            if (fieldInfo.FieldType == typeof(SettingVariable))
                settingsVariables.Add(fieldInfo.Name, fieldInfo);
        }

        string[] options2 = new string[settingsVariables.Keys.Count];
        settingsVariables.Keys.CopyTo(options2, 0);



        selected2 = EditorGUILayout.Popup("Selected Setting", selected2, options2);
        EditorGUILayout.LabelField(options[selected] + "." + options2[selected2], EditorStyles.helpBox);
        EditorGUILayout.Separator();



        selfController.settingVariable = (SettingVariable)settingsVariables[options2[selected2]].GetValue(settingsCategories[options[selected]]);



        serializedObject.Update();
        //EditorGUILayout.PropertyField(lookAtPoint);
        serializedObject.ApplyModifiedProperties();
        */
    }
    /*
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
    */


}
