using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VoxelSaveLoader : MonoBehaviour
{

    public TextAsset save;
    // Start is called before the first frame update
    void Start()
    {
        VoxelPalette.InitCategories();
        VoxelTools.LoadVoxelBuildFromFile(AssetDatabase.GetAssetPath(save), Vector3.zero, Vector3.zero);
        //Instantiate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
