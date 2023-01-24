using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;

[RootEditor]
public class AudioClipEditor : Editor
{


    [UnityEditor.MenuItem("Assets/Create Audio Clip Info")]
    private static void CreateAdioClipInfo()
    {
        AudioClipInfo audioClipInfo = ScriptableObject.CreateInstance("AudioClipInfo") as AudioClipInfo;
        audioClipInfo.audioClip = Selection.activeObject as AudioClip;
        //Selection.
        string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID())) +"/"+ Selection.activeObject.name + " - AudioClipInfo.asset";
        AssetDatabase.CreateAsset(audioClipInfo, path);
    }

    [UnityEditor.MenuItem("Assets/Create Audio Clip Info", true)]
    private static bool NewMenuOptionValidation()
    {
        // T$$anonymous$$s returns true when the selected object is a Variable (the menu item will be disabled otherwise).
        return Selection.activeObject is AudioClip;
    }

}
