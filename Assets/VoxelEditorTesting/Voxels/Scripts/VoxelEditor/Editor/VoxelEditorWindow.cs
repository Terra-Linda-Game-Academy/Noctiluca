using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System;

namespace Voxel.UI
{

public class VoxelEditorWindow : EditorWindow {

    string saveName = "";

    public Dictionary<Vector3Int, List<VoxelItem>> voxelMatrix = new Dictionary<Vector3Int, List<VoxelItem>>();

    [UnityEditor.MenuItem("Window/Voxel Editor")]
    public static void OpenWindow() {
        VoxelEditorWindow window = (VoxelEditorWindow)GetWindow(typeof(VoxelEditorWindow));
        window.minSize = new Vector2(100,200);
        window.Show();
    }

    string path = "Assets/resources/voxleFiles/";
    string[] filePaths = new string[0];


    void OnEnable() {
        Reload();
        VoxelPalette.InitCategories();
    }

    void Reload() 
    {
        InitTextures();


        
        filePaths = Directory.GetFiles(path, "*.json");
    }

    void InitTextures()
    {
    }

    [ContextMenu("Voxel Save Options")]
    void VoxelSaveOptionsMenu(string  filePath) {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Open"), false, () => {
            OpenSave(filePath);
        });
        
        menu.AddItem(new GUIContent("Delete"), false, () => {
            bool confirmed = EditorUtility.DisplayDialog("Confirm Delete", "Are you sure you want to delete this?", "Yes", "No");
            if(confirmed) {
                DeleteSave(filePath);
            }
            
        });

        
        menu.ShowAsContext();
    }

    public void OpenSave(string filePath) {
        VoxelSceneEditor.Instance.ClearScene();
        Dictionary<Vector3Int, List<VoxelData>> voxelItems = VoxelTools.LoadSceneVoxelsFromFile(filePath);
        foreach(Vector3Int pos in voxelItems.Keys) {
            foreach(VoxelData voxelData in voxelItems[pos]) {
                VoxelSceneEditor.Instance.CreateSceneVoxel(voxelData.voxelItem, pos, voxelData.rotation);
            }
        }
    }

    public void DeleteSave(string filePath) {
        AssetDatabase.DeleteAsset(filePath);
        AssetDatabase.Refresh();
        Reload();
    }



    public void OnGUI() {
        
        
        // titleContent.text = voxel.name + " (ID: "+voxel.id+")";
        // voxel.icon = (Texture2D)EditorGUILayout.ObjectField("Icon", voxel.icon, typeof(Texture2D), false);
        // voxel.color = EditorGUILayout.ColorField("Voxel Color", voxel.color);
        // voxel.prefab = (GameObject)EditorGUILayout.ObjectField("Refrence Prefab",  voxel.prefab, typeof(GameObject), false);

        foreach(string filePath in filePaths) {
            GUILayout.Box(filePath,  GUILayout.Width(Screen.width-5f), GUILayout.Height(Screen.height/10f));
            if(GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) &&  Event.current.type == EventType.MouseDown && Event.current.button == 0) {
                OpenSave(filePath);
                //Event.current.Use();
            }

            else if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)) {
                VoxelSaveOptionsMenu(filePath);
                //Event.current.Use();
            }
        }

        saveName = EditorGUILayout.TextField("Name", saveName);

        if(GUILayout.Button("Save",  GUILayout.Width(Screen.width-5f), GUILayout.Height(Screen.height/7f))) {
            //string path = "Assets/resources/voxels/"+voxel.name+".asset";
            //AssetDatabase.CreateAsset(voxel, path);
            //AssetDatabase.SaveAssets();

            // foreach(Vector3 v in VoxelSceneEditor.Instance.voxelMatrix.Keys) {
            //     Debug.Log("BPos: " + v);
            //      foreach(SceneVoxel voxel in VoxelSceneEditor.Instance.voxelMatrix[v]) {
                
            //         Debug.Log("BVoxel: " + voxel.voxelItem.name);
            //      }
            // }

            

            VoxelSceneEditor.Instance.RecountVoxels();
            Dictionary<Vector3Int, List<VoxelData>> voxelMatrix = VoxelTools.CenterVoxelBuild(VoxelTools.ConvertSceneVoxels(VoxelSceneEditor.Instance.voxelMatrix));

            //Debug.Log(VoxelSceneEditor.Instance.sceneVoxels.Count);
            VoxelTools.SaveSceneVoxelsToFile(voxelMatrix, path+saveName+".json");

            Reload();
            // foreach(Vector3 v in VoxelSceneEditor.Instance.voxelMatrix.Keys) {
            //     Debug.Log("APos: " + v);
            //      foreach(SceneVoxel voxel in VoxelSceneEditor.Instance.voxelMatrix[v]) {
                
            //         Debug.Log("AVoxel: " + voxel.voxelItem.name);
            //      }
            // }

            //voxelEditorWindow.Reload();

            //Close();

        }

        if(GUILayout.Button("Clear Scene",  GUILayout.Width(Screen.width-5f), GUILayout.Height(Screen.height/7f))) {
            bool confirmed = EditorUtility.DisplayDialog("Confirm Clear", "Are you sure you want to clear the scene? This can't be undone.", "Yes", "No");
            if(confirmed) {
                VoxelSceneEditor.Instance.ClearScene();
            }
            
        }

        if(GUILayout.Button("Place Starting Block",  GUILayout.Width(Screen.width-5f), GUILayout.Height(Screen.height/7f))) {
            if(VoxelSceneEditor.Instance.VoxelsAtPoint(Vector3Int.zero).Count == 0)
                VoxelSceneEditor.Instance.CreateSceneVoxel(VoxelPalette.BlockCategory.items[0], Vector3.zero);
            
        }
        // if(editingMode) {
        //     if(GUILayout.Button("Done",  GUILayout.Width(Screen.width-5f), GUILayout.Height(Screen.height/3.4f))) {
        //         //string path = "Assets/resources/voxels/"+voxel.name+".asset";
        //         //AssetDatabase.CreateAsset(voxel, path);
        //         //AssetDatabase.SaveAssets();

        //         voxelEditorWindow.Reload();

        //         Close();

        //     }
        // } else {
        //     if(GUILayout.Button("Create Voxel",  GUILayout.Width(Screen.width-5f), GUILayout.Height(Screen.height/3.4f))) {
        //         string path = VoxelPaletteWindow.GetVoxelPath(voxel.voxelCategory, voxel.name);
        //         AssetDatabase.CreateAsset(voxel, path);
        //         AssetDatabase.SaveAssets();

        //         voxelEditorWindow.Reload();

        //         Close();

        //     }
        // }

    
        
    }

}

public class VoxelPaletteWindow : EditorWindow
{

    Texture2D headerSectionTexture;
    Texture2D categorySectionTexture;
    Texture2D itemSectionTexture;
    Texture2D addItemTexture;

    Color headerSectionColor = new Color(120f/255f, 150f/255f, 200f/255f, 1f);
    Color categorySectionColor = new Color(90f/255f, 120f/255f, 140f/255f, 1f);
    Color itemSectionColor = new Color(75f/255f, 90f/255f, 100f/255f, 1f);

    Rect headerSection;
    Rect categorySection;
    Rect itemSection;

    int currentCategory = 0;

    private VoxelItem m_CurrentVoxel = null;
    public VoxelItem CurrentVoxel
    {
        get { return m_CurrentVoxel; }
        set { m_CurrentVoxel = value; 
        VoxelSceneEditor.Instance.CurrentVoxel = m_CurrentVoxel;}
    }
    

    Color selectionTintColor = new Color(10f/255f, 10f/255f, 50f/255f, 0.5f);
    Texture2D selectionTintTexture;


    Texture2D solidWhiteTexture;
    Texture2D solidBlackTexture;

    Texture2D transparentBlackTexture;


    [UnityEditor.MenuItem("Window/Voxel Palette")]
    static void OpenWindow() {
        VoxelPaletteWindow window = (VoxelPaletteWindow)GetWindow(typeof(VoxelPaletteWindow));
        window.minSize = new Vector2(10,10);
        window.Show();
    }

    bool subscribedToVoxelSceneEditor = false;

    void OnEnable() {
        
        Reload();

        titleContent.text = "Voxel Palette";

        EditorSceneManager.activeSceneChanged += OnSceneChanged;

        subscribedToVoxelSceneEditor = false;
    }


    private void OnDisable()
    {
        EditorSceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene currentScene, Scene nextScene)
    {
        Close();
    }

    public void Reload() {
        InitTextures();
        VoxelPalette.InitCategories();
        

        if(currentCategory > VoxelPalette.categories.Count-1)
            currentCategory = 0;

        //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    void InitTextures()
    {
        headerSectionTexture = new Texture2D(1,1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();

        categorySectionTexture = new Texture2D(1,1);
        categorySectionTexture.SetPixel(0, 0, categorySectionColor);
        categorySectionTexture.Apply();

        itemSectionTexture = new Texture2D(1,1);
        itemSectionTexture.SetPixel(0, 0, itemSectionColor);
        itemSectionTexture.Apply();

        selectionTintTexture = new Texture2D(1,1);
        selectionTintTexture.SetPixel(0, 0, selectionTintColor);
        selectionTintTexture.Apply();

        solidWhiteTexture = new Texture2D(1,1);
        solidWhiteTexture.SetPixel(0, 0, Color.white);
        solidWhiteTexture.Apply();

        solidBlackTexture = new Texture2D(1,1);
        solidBlackTexture.SetPixel(0, 0, Color.black);
        solidBlackTexture.Apply();

        
        transparentBlackTexture = new Texture2D(1,1);
        transparentBlackTexture.SetPixel(0, 0, new Color(0,0,0,0.75f));
        transparentBlackTexture.Apply();

        addItemTexture = Resources.Load<Texture2D>("icons/plus-icon");
    }

    

    



    
    void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawCategories();
        DrawItems();

        OnSceneGUI();
    }

    void OnSceneGUI() {
        if(!subscribedToVoxelSceneEditor && VoxelSceneEditor.Instance != null) {
            VoxelSceneEditor.Instance.OnVoxelItemChanged += () => {
                m_CurrentVoxel = VoxelSceneEditor.Instance.CurrentVoxel;
                currentCategory = VoxelPalette.categories.FindIndex((x) => x.voxelCategoryType==m_CurrentVoxel.voxelCategory);
            };
            subscribedToVoxelSceneEditor = true;
        }


        Vector3 mousePosition = Event.current.mousePosition;

        
    }

    GameObject cube;

 

    void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;

        categorySection.x = 0;
        categorySection.y = 50;
        categorySection.width = Screen.width;
        categorySection.height = Screen.height/5f;

        itemSection.x = 0;
        itemSection.y = 50 + Screen.height/5f;
        itemSection.width = Screen.width;
        itemSection.height = Screen.height/5f * 4 - 50f;

        
        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(categorySection, categorySectionTexture);
        GUI.DrawTexture(itemSection, itemSectionTexture);
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);

        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 45;
        myStyle.alignment = TextAnchor.MiddleCenter;

        GUILayout.Label("Voxels", myStyle);


        GUILayout.EndArea();
        
    }

    void DrawCategories()
    {
        GUIStyle myStyle = new GUIStyle();
        //myStyle.margin = new RectOffset(10,10,10,10);
        myStyle.padding = new RectOffset(10,10,10,10);
        

        GUILayout.BeginArea(categorySection, myStyle);
        GUILayout.Label("Categories");
        GUILayout.BeginHorizontal();

        float size = Screen.height/6f - 25f;


        foreach(VoxelCategory category in VoxelPalette.categories) {
            //GUISkin skin = new GUISkin();
            //skin.button

            GUIStyle categoryStyle = new GUIStyle();
            if(category == VoxelPalette.categories[currentCategory]) {
                categoryStyle.normal.background = solidWhiteTexture;
            } else {
                categoryStyle.normal.background = solidBlackTexture;
            }
            categoryStyle.normal.textColor = Color.white;
            categoryStyle.margin = new RectOffset(10,10,10,10);
            categoryStyle.fontSize = 17;
            categoryStyle.alignment = TextAnchor.MiddleCenter;
            categoryStyle.fontStyle = FontStyle.Bold;

            int maxFontSize = 32;
            int minFontSize = 4;

            int fontSize = Mathf.Min(Mathf.RoundToInt(Screen.height / 30f), Mathf.RoundToInt(size * size/Mathf.Max(GUI.skin.label.CalcSize(new GUIContent(category.name)).x, 1f)));
            fontSize = Mathf.Clamp(fontSize, Mathf.RoundToInt(minFontSize), Mathf.RoundToInt(maxFontSize));

            


            if(GUILayout.Button("", categoryStyle,  GUILayout.Width(size), GUILayout.Height(size))) {
                currentCategory = VoxelPalette.categories.IndexOf(category);
            }

            Rect iconRect = GUILayoutUtility.GetLastRect();
            iconRect.xMin+=size/15f;
            iconRect.xMax-=size/15f;

            iconRect.yMin+=size/15f;
            iconRect.yMax-=size/15f;
            GUI.DrawTexture(iconRect, category.icon);

            if(category == VoxelPalette.categories[currentCategory] ) {
                //Rect buttonRect = GUILayoutUtility.GetLastRect();
                
                GUI.DrawTexture(iconRect, selectionTintTexture);

                
                GUIStyle categoryTextStyle = new GUIStyle();
                categoryTextStyle.normal.textColor = Color.white;
                categoryTextStyle.fontSize = fontSize-3;
                categoryTextStyle.alignment = TextAnchor.MiddleCenter;
                categoryTextStyle.fontStyle = FontStyle.Bold;
                GUI.Label(iconRect, new GUIContent(category.name), categoryTextStyle);
            } else {
                iconRect.yMin+=size/4f;
                iconRect.yMax-=size/4f;


                
                GUI.DrawTexture(iconRect, transparentBlackTexture);

                GUIStyle categoryTextStyle = new GUIStyle();
                categoryTextStyle.normal.textColor = Color.white;
                categoryTextStyle.fontSize = fontSize;
                categoryTextStyle.alignment = TextAnchor.MiddleCenter;
                categoryTextStyle.fontStyle = FontStyle.Bold;
                GUI.Label(iconRect, new GUIContent(category.name), categoryTextStyle);
            }
            
            
            //Rect categoryRect = new Rect(new);
            //GUI.DrawTexture(headerSection, headerSectionTexture);
        }


        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void DeleteVoxel(VoxelItem voxel) {
        foreach(SceneVoxel sv in VoxelSceneEditor.Instance.sceneVoxels.FindAll((x) => x.voxelItem == voxel)) {
            VoxelSceneEditor.Instance.RemoveVoxel(sv);
        }

        string path = AssetDatabase.GetAssetPath(voxel);
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.Refresh();
        Reload();
    }

    [ContextMenu("Voxel Options")]
    void VoxelOptionsMenu( VoxelItem voxel) {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Edit"), false, () => {
            NewVoxelWindow.OpenWindow(this, voxel);
        });
        menu.AddItem(new GUIContent("Duplicate"), false, () => {
            string filePath =  AssetDatabase.GetAssetPath(voxel);
            
            NewVoxelWindow.OpenWindow(this, DuplicateVoxel(filePath, voxel.name + "(2)"));
        });

        
        menu.AddItem(new GUIContent("Delete"), false, () => {
            bool confirmed = EditorUtility.DisplayDialog("Confirm Delete", "Are you sure you want to delete this?", "Yes", "No");
            if(confirmed) {
                DeleteVoxel(voxel);
            }
            
        });
        menu.ShowAsContext();
    }

    


    public VoxelItem DuplicateVoxel(string filePath, string newName) {
        

        string newPath = Path.GetDirectoryName(filePath) +"\\"+ newName + ".asset";
        Debug.Log("Path: " + newPath);
        AssetDatabase.CopyAsset(filePath, newPath);
        VoxelItem voxelItem = AssetDatabase.LoadAssetAtPath<VoxelItem>(newPath);
        voxelItem.name = newName;
        voxelItem.id = VoxelTools.GetNewID();
        AssetDatabase.Refresh();
        Reload();

        return voxelItem;
    }


    string search = "";
    public Vector2 scrollPosition;
    void DrawItems()
    {

        GUIStyle myStyle = new GUIStyle();
        myStyle.margin = new RectOffset(10,10,10,10);
        myStyle.padding = new RectOffset(10,10,10,10);
        

        GUILayout.BeginArea(itemSection, myStyle);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Items");
        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Search: ");
        search = GUILayout.TextField(search);
        //GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        //Block ID's are getting ovveriden and files a broken, make ID generation random!!
        
        float size = Screen.height/5f - 25f;
        float currentX = 0;
        foreach(VoxelItem item in VoxelPalette.categories[currentCategory].items.FindAll((x) => x.name.ToLower().Contains(search.ToLower()))) {
            if(currentX == 0) {
                GUIStyle horizontalStyle = new GUIStyle();
                horizontalStyle.stretchWidth = true;
                GUILayout.BeginHorizontal(horizontalStyle);
                //GUILayout.FlexibleSpace();

            }

            // if(GUILayout.Button(new GUIContent(item.name, item.icon), GUILayout.Width(size), GUILayout.Height(size))) {

            // }
            //EditorGUILayout.Popup(new GUIContent("movementStepToMoveTo", "YOUR TOOLTIP HERE"));
            
            //GUILayout.FlexibleSpace();
            

            GUIStyle itemStyle = new GUIStyle();
            if(item == CurrentVoxel) {
                itemStyle.normal.background = solidWhiteTexture;
            } else {
                itemStyle.normal.background = solidBlackTexture;
            }
            itemStyle.normal.textColor = Color.white;
            itemStyle.margin = new RectOffset(10,10,10,10);
            itemStyle.fontSize = (int)size;
            itemStyle.alignment = TextAnchor.MiddleCenter;
            itemStyle.fontStyle = FontStyle.Bold;

            int maxFontSize = 25;
            int minFontSize = 4;


            int fontSize = Mathf.Min(Mathf.RoundToInt(Screen.height / 30f), Mathf.RoundToInt(maxFontSize * size /(Mathf.Max(GUI.skin.label.CalcSize(new GUIContent(item.name)).x, 1f)+size)));
            fontSize = Mathf.Clamp(fontSize, Mathf.RoundToInt(minFontSize), Mathf.RoundToInt(maxFontSize));


            

            GUILayout.Box("", itemStyle,  GUILayout.Width(size), GUILayout.Height(size));
            Rect actualRect = GUILayoutUtility.GetLastRect();
            if(Event.current.type == EventType.MouseDown && Event.current.button == 0 && actualRect.Contains(Event.current.mousePosition)) {
                CurrentVoxel = item;
                Event.current.Use();
            }

            

            Rect iconRect = actualRect;
            iconRect.xMin+=size/20f;
            iconRect.xMax-=size/20f;

            iconRect.yMin+=size/20f;
            iconRect.yMax-=size/20f;

            if(item.icon != null)
                GUI.DrawTexture(iconRect, item.icon);

            if(item == CurrentVoxel ) {
                //Rect buttonRect = GUILayoutUtility.GetLastRect();
                
                GUI.DrawTexture(iconRect, selectionTintTexture);

                
                GUIStyle categoryTextStyle = new GUIStyle();
                categoryTextStyle.normal.textColor = Color.white;
                //(size/5f)
                categoryTextStyle.fontSize = fontSize-3;
                categoryTextStyle.alignment = TextAnchor.MiddleCenter;
                categoryTextStyle.fontStyle = FontStyle.Bold;
                GUI.Label(iconRect, new GUIContent(item.name), categoryTextStyle);
            } else {
                iconRect.yMin+=size/4f;
                iconRect.yMax-=size/4f;


                
                GUI.DrawTexture(iconRect, transparentBlackTexture);

                GUIStyle categoryTextStyle = new GUIStyle();
                categoryTextStyle.normal.textColor = Color.white;
                categoryTextStyle.fontSize = fontSize;
                categoryTextStyle.alignment = TextAnchor.MiddleCenter;
                categoryTextStyle.fontStyle = FontStyle.Bold;
                GUI.Label(iconRect, new GUIContent(item.name), categoryTextStyle);
            }

            if (Event.current.type == EventType.ContextClick && actualRect.Contains(Event.current.mousePosition)) {
                VoxelOptionsMenu(item);
                Event.current.Use();
                //Repaint();
            }

            currentX += size;

            if(currentX + size > Screen.width - size/2f) {
                currentX = 0;
                GUILayout.EndHorizontal();
            }
        }

        //PLUS BUTTON
        if(currentX == 0)
        {
            GUILayout.BeginHorizontal();
            
        }

        GUIStyle addNewStyle = new GUIStyle();
        addNewStyle.normal.textColor = Color.white;
        //addNewStyle.normal.background = addItemTexture;
        addNewStyle.margin = new RectOffset((int)(size/3f),(int)(size/3f),(int)(size/3f),(int)(size/3f));
        addNewStyle.fontSize = (int)size;
        addNewStyle.alignment = TextAnchor.MiddleCenter;
        addNewStyle.fontStyle = FontStyle.Bold;

        
        if(GUILayout.Button("",addNewStyle,  GUILayout.Width(size/2f), GUILayout.Height(size/2f))) {
            NewVoxelWindow.OpenWindow(this, VoxelPalette.categories[currentCategory].voxelCategoryType);
        }

        Rect plusRect = GUILayoutUtility.GetLastRect();

        Vector2 mousePosition = Event.current.mousePosition;
        if(plusRect.Contains(mousePosition)) {
            plusRect.xMin-=size/20f;
            plusRect.xMax+=size/20f;

            plusRect.yMin-=size/20f;
            plusRect.yMax+=size/20f;

            //plusRect = new Rect(100f,100f,100f,100f);
        }


        GUI.DrawTexture(plusRect, addItemTexture);

        Repaint();

        currentX += size;

        if(currentX != 0)
            GUILayout.EndHorizontal();


        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    

    public static string GetVoxelPath(VoxelCategoryType voxelCategory, string name) {
        return "Assets/resources/voxels/"+voxelCategory.ToString() +"/"+name+".asset";
    }

}



// class VoxelItem {
//     public string name, id;
//     public Texture2D icon;
//     public VoxelItem(string name, Texture2D icon, string id) {
//         this.name = name;
//         this.icon = icon;
//         this.id = id;
//     }

// }


public class NewVoxelWindow : EditorWindow {


    VoxelItem voxel;

    public bool editingMode = false;

    private Texture2D previewImage;


    public VoxelPaletteWindow voxelPaletteWindow;

    public static void OpenWindow(VoxelPaletteWindow voxelPaletteWindow, VoxelCategoryType voxelCategoryType) {
        NewVoxelWindow window = (NewVoxelWindow)GetWindow(typeof(NewVoxelWindow));
        window.minSize = new Vector2(100,200);
        window.Show();

        window.voxelPaletteWindow = voxelPaletteWindow;
        window.CreateNewVoxel();
        window.voxel.voxelCategory = voxelCategoryType;
        window.editingMode = false;
        window.Init();
    }

    public static void OpenWindow(VoxelPaletteWindow voxelPaletteWindow, VoxelItem voxel) {
        NewVoxelWindow window = (NewVoxelWindow)GetWindow(typeof(NewVoxelWindow));
        window.minSize = new Vector2(300,250);
        window.Show();

        window.voxelPaletteWindow = voxelPaletteWindow;
        window.voxel = voxel;
        window.editingMode = true;
        window.Init();
    }

    string originalName = "";

    public void CreateNewVoxel() {
       voxel = ScriptableObject.CreateInstance<VoxelItem>();
       voxel.id = VoxelTools.GetNewID();
    }

    public void ResaveVoxel(VoxelItem voxelItem) {
        string path = AssetDatabase.GetAssetPath(voxelItem);
        AssetDatabase.RenameAsset(path, voxelItem.name);
        AssetDatabase.Refresh();
        voxelPaletteWindow.Reload();
    }

    public void OnDisable() {
        Debug.Log("On Disable");
        if(voxel.name != originalName)
            ResaveVoxel(voxel);
    }

    public void Init() {
        originalName = voxel.name;


        //previewImage = PrefabPreview();
        //voxel.OnVoxelItemUpdated += () => {previewImage = PrefabPreview();};
    }


    public void OnGUI() {
        //voxel.voxelCategory = (VoxelCategoryType)EditorGUILayout.EnumPopup("Voxel Type", voxel.voxelCategory);
        voxel.name = EditorGUILayout.TextField("Name", voxel.name);
        titleContent.text = voxel.name + " (ID: "+voxel.id+")";
        //voxel.id = EditorGUILayout.IntField("ID", voxel.id);
        voxel.icon = (Texture2D)EditorGUILayout.ObjectField("Icon", voxel.icon, typeof(Texture2D), false);
        voxel.color = EditorGUILayout.ColorField("Voxel Color", voxel.color);
        voxel.prefab = (GameObject)EditorGUILayout.ObjectField("Refrence Prefab",  voxel.prefab, typeof(GameObject), false);
        voxel.scale = EditorGUILayout.Vector3Field("Prefab Scale", voxel.scale);
        voxel.offset = EditorGUILayout.Vector3Field("Prefab Offset", voxel.offset);

        //GUI.DrawTexture(new Rect(100,100,100,100), previewImage);

        // switch(voxel.voxelCategory) {

        // }

        if(editingMode) {
            if(GUILayout.Button("Done",  GUILayout.Width(Screen.width-5f), GUILayout.Height(Screen.height/3.4f))) {
                //string path = "Assets/resources/voxels/"+voxel.name+".asset";
                //AssetDatabase.CreateAsset(voxel, path);
                //AssetDatabase.SaveAssets();

                voxelPaletteWindow.Reload();

                Close();

            }
        } else {
            if(GUILayout.Button("Create Voxel",  GUILayout.Width(Screen.width-5f), GUILayout.Height(Screen.height/3.4f))) {
                string path = VoxelPaletteWindow.GetVoxelPath(voxel.voxelCategory, voxel.name);
                AssetDatabase.CreateAsset(voxel, path);
                AssetDatabase.SaveAssets();

                voxelPaletteWindow.Reload();

                Close();

            }
        }

    

        // GUILayout.Label("Prefab Viewer", EditorStyles.boldLabel);

        // prefabObject = EditorGUILayout.ObjectField("Prefab", prefabObject, typeof(GameObject), false) as GameObject;

        // if (prefabObject != null && GUILayout.Button("Open Prefab Viewer"))
        // {
            
        // }
        
        
        
    }

}

}
