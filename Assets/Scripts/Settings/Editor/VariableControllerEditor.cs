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

    
    
    public override void OnInspectorGUI()
    {


        DrawDefaultInspector();

        
        //Debug.Log(selected[0]);
        VariableController selfController = (VariableController)target;

        selfController.LayerLength = EditorGUILayout.IntField("Reflection Distance", selfController.LayerLength);

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

        FieldInfo currentField = null;

        while (stillMoreLayers && layer < selfController.LayerLength)
        {
            //EditorGUILayout.LabelField("Selected Variable", EditorStyles.boldLabel);
            //category
           // Debug.Log("Type: " + currentType);
            Dictionary<string, FieldInfo> settingVariables = new Dictionary<string, FieldInfo>();
            foreach (FieldInfo fieldInfo in currentType.GetFields( BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static |
                         BindingFlags.Instance))
            {
                settingVariables.Add(fieldInfo.Name, fieldInfo); 
            }

            string[] options = new string[settingVariables.Keys.Count];
            settingVariables.Keys.CopyTo(options, 0);
            selfController.selected[layer] = EditorGUILayout.Popup("Selected Category", selfController.selected[layer], options);

            Type fieldType = settingVariables[options[selfController.selected[layer]]].FieldType;
            currentField = settingVariables[options[selfController.selected[layer]]];

            layers.Add(currentField);
            

            directory += currentField.Name;
            if (fieldType.GetFields().Length > 0 && fieldType != typeof(SettingVariable) &&  layer < selfController.LayerLength)
            {
                //Continue
                stillMoreLayers = true;
                currentType = fieldType;
                
                directory += ".";
            }
             else
            {
                stillMoreLayers = false;
            }
            layer++;
        }


        if(layer > 0) {
            object previousLayerObject = instance;
            foreach(FieldInfo fieldInfo in layers)
            {
                previousLayerObject = fieldInfo.GetValue(previousLayerObject);
            }
            selfController.Variable = previousLayerObject;


            EditorGUILayout.LabelField(currentField.FieldType.Name +" "+ directory, EditorStyles.helpBox);

            string valueText = previousLayerObject.ToString();
            if(previousLayerObject is SettingVariable) {
                valueText = ((SettingVariable)previousLayerObject).Value.ToString();
            }
            EditorGUILayout.LabelField("Value: " + valueText, EditorStyles.helpBox);
            
        }

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
