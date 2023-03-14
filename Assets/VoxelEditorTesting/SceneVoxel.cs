using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[ExecuteInEditMode]
public class SceneVoxel : MonoBehaviour
{
    public Renderer meshRenderer;
    private Material material;
    
    public Material secondaryMaterial; 

    private GameObject previewObject;
    private bool m_PreviewMode;
    public bool PreviewMode
    {
        get { return m_PreviewMode; }
        set { 
            

            if(m_PreviewMode == value)
                return;
            
            

            m_PreviewMode = value;

            if(previewObject == null)
                GeneratePreview();

            if(previewObject != null)
             {
                previewObject.SetActive(m_PreviewMode);
             }
             
                
            meshRenderer.enabled = (!m_PreviewMode);
        }
    }


    public bool previewPlaceMode = false;

    private bool m_DestroyMode;
    public bool DestroyMode
    {
        get { return m_DestroyMode; }
        set { m_DestroyMode = value; 
        
        if(m_DestroyMode) {
            ChangeMaterialColor(new Color(1f, 0f, 0f, 0.4f));
        } else {
            ChangeMaterialColor(voxelItem.color);
        }
        }
    }
    
    public VoxelItem voxelItem;


    public void SubscribeToVoxelUpdate() {
        voxelItem.OnVoxelItemUpdated -= Refresh;
        voxelItem.OnVoxelItemUpdated += Refresh;
    }


    private void GeneratePreview(bool disable = false) {

        if(previewPlaceMode)
            return;
        
        if (previewObject != null && PrefabUtility.GetCorrespondingObjectFromSource(previewObject) == voxelItem.prefab)
        {
            // previewObject.transform.parent = transform;
            // previewObject.transform.localScale = voxelItem.scale;
            // previewObject.transform.localPosition = voxelItem.offset;
            return;
        }



        if(previewObject != null) {
            DestroyImmediate(previewObject);
        }

        for(int i = 0; i < gameObject.transform.childCount; i++) {
             DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
        }
            
        
        if(voxelItem.prefab != null) {
            previewObject = Instantiate(voxelItem.prefab, transform.position, transform.rotation);
        } else {
            previewObject = new GameObject("Empty Preview");
        }

        if(previewObject!=null && previewObject.transform != null) {
            previewObject.transform.parent = transform;
        }
            
        TransformPreview();

        if(disable)
            previewObject.SetActive(false);
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        if(!previewPlaceMode)
            SubscribeToVoxelUpdate();
        //EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private void OnDisable()
    {
        
        //EditorApplication.hierarchyChanged -= OnHierarchyChanged;
    }

    private void OnHierarchyChanged()
    {
        //Init();
    }

    private void Init() {
        
        meshRenderer = GetComponent<MeshRenderer>();
        material = new Material(Shader.Find("Standard"));
        meshRenderer.material = material;

        if(voxelItem == null)
            ChangeMaterialColor(Color.magenta);
        else if(previewPlaceMode) {
            ChangeMaterialColor(new Color(0f, 1f, 0f, 0.4f));
            GetComponent<Collider>().enabled = false;
        } else {
            
            GeneratePreview(true);
            

            ChangeMaterialColor(voxelItem.color);

            SubscribeToVoxelUpdate();

            
        }
        
        
    }

    public void Refresh(VoxelItem voxelItem, string variableName) {
        if(previewPlaceMode)
            return;


        if(meshRenderer == null) {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        

        /*
        try{

            string value = "";

            MethodInfo f = typeof(VoxelItem).GetMethod("get_"+variableName);
            value = f.Invoke((object)voxelItem, new object[]{}).ToString();

            Debug.Log("Refreshing: " + variableName + " to " + value);
        }catch(Exception e){ Debug.LogError(e.Message);}
        */

        if(variableName == "color" || variableName == "all")
            ChangeMaterialColor(voxelItem.color);
        

        if(variableName == "prefab" || variableName == "all") {
            GeneratePreview();
        }
            
        else if(variableName == "scale" || variableName == "offset" || variableName == "prefab" || variableName == "all")
            TransformPreview();


        if(previewObject != null)
        {
            previewObject.SetActive(m_PreviewMode);
        }
            
            
        meshRenderer.enabled = (!m_PreviewMode);



    }

    void TransformPreview() {
        if(previewObject == null)
            return;
        previewObject.transform.localScale = voxelItem.scale;
        previewObject.transform.localPosition = voxelItem.offset;
    }

    private void OnDestroy() {
        if(VoxelSceneEditor.Instance.sceneVoxels.Contains(this))
            VoxelSceneEditor.Instance.RemoveVoxel(this, false);

    }

    private void Update() {
        if(previewPlaceMode && VoxelSceneEditor.Instance.voxelPlacePreview != gameObject) {
            DestroyImmediate(gameObject);
        }
    }

    public void AddSecondaryMaterial(Material mat) {
        Material[] materials = new Material[2];
        materials[0] = meshRenderer.sharedMaterial;
        materials[1] = mat;
        meshRenderer.sharedMaterials = materials;
    }

    public void RemoveSecondaryMaterial() {
        meshRenderer.sharedMaterials = new Material[1] {meshRenderer.sharedMaterial};
    }


    void ChangeMaterialColor(Color color)
{
    // Check if the color is transparent
    if (color.a < 1.0f)
    {
        // Set the material's color and enable transparency
        material.SetColor("_Color", new Color(color.r, color.g, color.b, color.a));
        material.SetFloat("_Mode", 3.0f);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }
    else
    {
        // Set the material's color without transparency
        material.SetColor("_Color", color);
        material.SetFloat("_Mode", 0.0f);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }
}




}
