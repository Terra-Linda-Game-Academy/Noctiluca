using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public delegate void VoxelItemChangedDelegate();

[ExecuteInEditMode, RequireComponent(typeof(UnityEngine.Camera))]
public class VoxelSceneEditor : MonoBehaviour {
//
    public UnityEngine.Camera cam;

    public SceneVoxel voxelPrefab;


    public BuildMode buildMode = BuildMode.NONE;

    public List<SceneVoxel> sceneVoxels = new List<SceneVoxel>();
    //                      x y z
    private Dictionary<Vector3Int, List<SceneVoxel>> m_voxelMatrix = new Dictionary<Vector3Int, List<SceneVoxel>>();
    public Dictionary<Vector3Int, List<SceneVoxel>> voxelMatrix
    {
        get { return m_voxelMatrix; }
        set { m_voxelMatrix = value; }
    }
    

    Vector3 voxelPlacePreviewRotation = Vector3.zero;
    public SceneVoxel voxelPlacePreview;
    public bool editingPreviewEnabled = false;

    public bool fullPreviewEnabled = false;
    public bool showRotationEnabled = false;

    public Material rotationMaterial;



    private SceneVoxel m_VoxelDestroyPreview;
    public SceneVoxel VoxelDestroyPreview
    {
        get { return m_VoxelDestroyPreview; }
        set { 
            if(m_VoxelDestroyPreview != null)
                m_VoxelDestroyPreview.DestroyMode = false;
            m_VoxelDestroyPreview = value; 
            if(m_VoxelDestroyPreview != null)
                m_VoxelDestroyPreview.DestroyMode = true;
        }
    }

    


    private VoxelItem m_currentVoxel;
    public VoxelItem CurrentVoxel
    {
        get { return m_currentVoxel; }
        set { m_currentVoxel = value; if(OnVoxelItemChanged != null) OnVoxelItemChanged();}
    }
    

    public static VoxelSceneEditor Instance;
    

    public event VoxelItemChangedDelegate OnVoxelItemChanged;

    Texture2D blackTexture;


    

    #region Unity Methods

    #if UNITY_EDITOR

   
    void OnEnable()
    {
        
        Instance = this;
        cam = GetComponent<UnityEngine.Camera>();

        blackTexture = new Texture2D(1,1);
        blackTexture.SetPixel(0, 0, new Color(0,0,0,0.75f));
        blackTexture.Apply();    

        SceneView.duringSceneGui += OnScene;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        OnVoxelItemChanged += UpdateVoxel;


        foreach(SceneVoxel sv in GameObject.FindObjectsOfType<SceneVoxel>()) {
            if(!sceneVoxels.Contains(sv)) {
                AddVoxel(sv);
            }
        }
        
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnScene;
    }

    void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            SceneView.duringSceneGui -= OnScene;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

            try{
                DestroyImmediate(gameObject);
            } catch {}
            
        }
    }




    void UpdateVoxel() {
        //CurrentVoxel.OnVoxelItemUpdated += RefreshVoxels;
    }

    void RefreshVoxels() {
        foreach(SceneVoxel sv in sceneVoxels) {
            sv.Refresh(sv.voxelItem, "all");
        }

        RecountVoxels();
    }

    public void RecountVoxels() {
        sceneVoxels.Clear();
        m_voxelMatrix.Clear();
        foreach(SceneVoxel sv in GameObject.FindObjectsOfType<SceneVoxel>()) {
            AddVoxel(sv);
        }
    }

    bool MatrixContains(SceneVoxel sv) {
        foreach(List<SceneVoxel> sceneVoxels in m_voxelMatrix.Values) {
            if(sceneVoxels.Contains(sv))
                return true;
        }
        return false;
    }

    
    public List<SceneVoxel> VoxelsAtPoint(Vector3Int point) {
        try {
            return m_voxelMatrix[point];
        } catch {
            return new List<SceneVoxel>();
        }
        
    }


    public Vector3Int GetVoxelPos(SceneVoxel sv) {
        Vector3 pos = sv.transform.position;
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
       
        return new Vector3Int(x,y,z);
    }

    public void AddVoxel(SceneVoxel sv) {
        
        Vector3Int pos = GetVoxelPos(sv);
        // Debug.Log("Got pos " + pos);
        if(m_voxelMatrix.ContainsKey(pos)) {
            m_voxelMatrix[pos].Add(sv);
        } else {
            List<SceneVoxel> voxels = new List<SceneVoxel>();
            voxels.Add(sv);
            m_voxelMatrix.Add(pos, voxels);
        }

        if(!sceneVoxels.Contains(sv))
            sceneVoxels.Add(sv);
        
    }

    public void RemoveVoxel(SceneVoxel sv, bool destroy = true) {

        Vector3Int pos = GetVoxelPos(sv);
        if(m_voxelMatrix.ContainsKey(pos) && m_voxelMatrix[pos].Contains(sv)) {
            m_voxelMatrix[pos].Remove(sv);
        }

        sceneVoxels.Remove(sv);

        if(destroy)
            DestroyImmediate(sv.gameObject);
    }
    public void ClearScene() {

        foreach(SceneVoxel sv in sceneVoxels.ToArray()) {
            RemoveVoxel(sv);
        }
    }
    


    void DrawTitle(string title) {
        //Voxel Mode UI
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 25;
        Rect rect = new Rect(10, 10, GUI.skin.label.CalcSize(new GUIContent(title)).x*2, 30);
        Handles.BeginGUI();
        Rect largerRect = rect;
        largerRect.xMin-=5f;
        largerRect.xMax+=5f;
        GUI.DrawTexture(largerRect, blackTexture);
        GUI.Label(rect, title, style);
        Handles.EndGUI();
    }

    bool GetHit(out RaycastHit hit) {
        Vector3 mousePosition = Event.current.mousePosition;

        float perPixel = UnityEditor.EditorGUIUtility.pixelsPerPoint;
        mousePosition.y = UnityEngine.Camera.current.pixelHeight - mousePosition.y * perPixel;
        mousePosition.x *= perPixel;

        Ray ray = UnityEngine.Camera.current.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return true;
        }
        else return false;
    }

    void ClearSlection() {
        Selection.objects = new GameObject[0];
    }

    Vector3 GetGridPosition(RaycastHit hit) {
        // Snap the new object to the center of the hit face
        float gridSize = 1f; // Change this to adjust the size of the grid
        Vector3 snapPoint = hit.point + (hit.normal * (gridSize / 2f));

        // Snap to the nearest grid point
        snapPoint = new Vector3(
            Mathf.Round(snapPoint.x / gridSize) * gridSize,
            Mathf.Round(snapPoint.y / gridSize) * gridSize,
            Mathf.Round(snapPoint.z / gridSize) * gridSize
        );
        return snapPoint;
    }

    

    public void CreateSceneVoxel(RaycastHit hit, VoxelItem voxelItem, Vector3 rotation = new Vector3()) {
        CreateSceneVoxel(voxelItem,GetGridPosition(hit),  rotation);
    }

    public void CreateSceneVoxel(VoxelItem voxelItem, Vector3 point, Vector3 rotation = new Vector3()) {
        voxelPrefab.voxelItem = voxelItem;
        voxelPrefab.previewPlaceMode = false;
        SceneVoxel voxel = Instantiate(voxelPrefab.gameObject).GetComponent<SceneVoxel>();

        voxel.gameObject.name = voxelItem.name;

        voxel.PreviewMode = fullPreviewEnabled;

        voxel.transform.parent = gameObject.transform;

        voxel.transform.position = point;

        if(rotation.magnitude > 0)
            voxel.transform.rotation = Quaternion.Euler(rotation);

        AddVoxel(voxel);
    }

    void OnScene(SceneView sceneview)
    {

        if(voxelPlacePreview == null) {
            voxelPrefab.previewPlaceMode = true;
            voxelPlacePreview = Instantiate(voxelPrefab.gameObject).GetComponent<SceneVoxel>();
            voxelPlacePreview.AddSecondaryMaterial(rotationMaterial);
        }
    
        switch(buildMode) {
            case BuildMode.NONE:
                VoxelDestroyPreview = null;
                voxelPlacePreview.gameObject.SetActive(false);
                break;
            case BuildMode.PLACE:
                DrawTitle("Place Mode");
                
                ClearSlection();
                VoxelDestroyPreview = null;

                //Draw Preview
                if(editingPreviewEnabled) {
                    RaycastHit hit;
                    if (GetHit(out hit))
                    {
                        voxelPlacePreview.gameObject.SetActive(true);
                        
                        voxelPlacePreview.transform.position = GetGridPosition(hit);
                    } else {
                        voxelPlacePreview.gameObject.SetActive(false);
                    }
                } else {
                    voxelPlacePreview.gameObject.SetActive(false);
                }
                //

                if (Event.current != null && CurrentVoxel != null)
                {
                    if (Event.current.type == EventType.MouseDown  && Event.current.button == 0)
                    {
                        RaycastHit hit;
                        if (GetHit(out hit))
                        {
                           CreateSceneVoxel(hit, CurrentVoxel, voxelPlacePreview.transform.rotation.eulerAngles);
                            //SceneVoxel sceneVoxel = voxelObject.GetComponent<SceneVoxel>();

                            

                            Event.current.Use();
                        }
                    }   else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C)
                    {
                        Vector3 mousePosition = Event.current.mousePosition;
                        RaycastHit hit;
                        if (GetHit(out hit))
                        {
                            SceneVoxel sceneVoxel = hit.transform.gameObject.GetComponent<SceneVoxel>();
                            if(sceneVoxel != null) {
                                CurrentVoxel = sceneVoxel.voxelItem;
                                Event.current.Use();
                            }
                        }
                    }
                }
                break;
            case BuildMode.DESTROY:
                DrawTitle("Destroy Mode");

                voxelPlacePreview.gameObject.SetActive(false);

                ClearSlection();

                //Draw Preview
                if(editingPreviewEnabled) {
                    RaycastHit hit;
                    if (GetHit(out hit))
                    {
                        SceneVoxel sceneVoxel = hit.transform.gameObject.GetComponent<SceneVoxel>();
                        if(sceneVoxel != null) {
                            VoxelDestroyPreview = sceneVoxel;
                            //DestroyImmediate(sceneVoxel);
                            //sceneVoxel.destroyMode = true;
                        }
                    } else {
                        VoxelDestroyPreview = null;
                    }
                } else {
                    VoxelDestroyPreview = null;
                }
                //

                if (Event.current != null && CurrentVoxel != null)
                {
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        RaycastHit hit;
                        if (GetHit(out hit))
                        {
                            SceneVoxel sceneVoxel = hit.transform.gameObject.GetComponent<SceneVoxel>();
                            if(sceneVoxel != null) {
                                RemoveVoxel(sceneVoxel);
                                Event.current.Use();
                            }
                        }
                        
                    }
                }
                break;
            
        }

        if(fullPreviewEnabled) {
             DrawTitle("Preview Mode");
        }

        voxelPlacePreview.transform.rotation = Quaternion.Euler(voxelPlacePreviewRotation);
        
        

        

        

        if (Event.current != null)
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.P) {
                editingPreviewEnabled = !editingPreviewEnabled;
                Event.current.Use();
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.G) {
                fullPreviewEnabled = !fullPreviewEnabled;

                foreach(SceneVoxel sv in sceneVoxels) {
                    if(sv != null)
                        sv.PreviewMode = fullPreviewEnabled;
                }
                Event.current.Use();
            }

             if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.B) {
                showRotationEnabled = !showRotationEnabled;

                //Debug.Log(":)");

                foreach(SceneVoxel sv in sceneVoxels) {
                    if(sv != null) {
                        if(showRotationEnabled) {
                            sv.AddSecondaryMaterial(rotationMaterial);
                        } else {
                            sv.RemoveSecondaryMaterial();
                        }
                    }
                        
                }
                Event.current.Use();
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.E) {
                Debug.Log("Rotate");
                voxelPlacePreviewRotation += new Vector3(0,90,0);
                Event.current.Use();
            } else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Q) {
                Debug.Log("Rotate");
                voxelPlacePreviewRotation += new Vector3(90,0,0);
                Event.current.Use();
            }
            

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.R) {

                RefreshVoxels();
                Event.current.Use();
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) {
                buildMode = BuildMode.NONE;
                Event.current.Use();
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab) {
                CycleMode();
                Event.current.Use();
            }

            //Debug.Log("Current code: " + Event.current.button);
              

        }
    }

    void CycleMode() {
        switch (buildMode)
        {
            case BuildMode.NONE:
                buildMode = BuildMode.PLACE;
                break;
            case BuildMode.PLACE:
                buildMode = BuildMode.DESTROY;
                break;
            case BuildMode.DESTROY:
                buildMode = BuildMode.PLACE;
                break;
            default:
                break;
        }
    }




    #endif

    #endregion
}

public enum BuildMode { NONE, PLACE, DESTROY }