using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(MenuSection))]
public class MenuSectionEditor : Editor
{
    // public override void OnInspectorGUI()
    // {
    //     DrawDefaultInspector();

        
    //     MenuSection menuSectionScript = (MenuSection)target;

    //     //menuSectionScript.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
    //     foreach (FieldInfo fieldInfo in typeof(MenuItem).GetFields()) {
    //         Debug.Log(fieldInfo.Name);
    //         if(fieldInfo.GetCustomAttribute<SerializeField>() != null || (fieldInfo.GetCustomAttribute<HideInInspector>() == null && fieldInfo.IsPublic)) {
                
    //             foreach (MethodInfo methodInfo in typeof(EditorGUILayout).GetMethods()) {
    //                 if (methodInfo.Name.Contains("Field") && methodInfo.GetParameters().Length == 2 && methodInfo.GetParameters()[0].ParameterType == typeof(string) && methodInfo.GetParameters()[1].ParameterType == fieldInfo.FieldType)
    //                 {
    //                     EditorGUILayout.IntField("Experience", 0);
    //                     methodInfo.Invoke(null, new object[] { fieldInfo.Name, fieldInfo.GetValue(menuSectionScript) });
    //                 }
                    
    //             }
    //         }
    //     }

    //     if(GUILayout.Button("Auto Animate Buttons"))
    //     {
    //         //menuSectionScript.AnimateAllButtons();
    //     }
    // }
}
