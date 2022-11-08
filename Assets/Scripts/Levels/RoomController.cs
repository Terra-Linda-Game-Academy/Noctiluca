using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Levels {
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class RoomController : MonoBehaviour {
        [SerializeField] private Room assetTarget;
        
        public Guid RoomId { get; private set; }
        
        private Vector3Int dimensions;
        private byte[] heightMap;
        private Terrain terrain;
        
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        private List<ITile> tiles;
        
        
        private void Awake() {
            #if UNITY_EDITOR 
            EditorSceneManager.sceneSaving += OnSceneSave;
            #endif
            RoomId = new Guid();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start() {
            foreach (var tile in tiles) {
                GameObject obj = new GameObject(tile.Name);
                Transform objTransform = obj.transform;
                objTransform.parent = transform;
                objTransform.position = tile.Position + new Vector3(0.5f, 0f, 0.5f);
                tile.Init(obj, RoomId);
            }
        }

        private void SetupTerrainRenderer() {
            meshRenderer.sharedMaterials = new Material[] { }; //todo: grab materials from Terrain
        }

        private void RefreshTerrainMesh() {
            
        }

        private void OnDestroy() {
            #if UNITY_EDITOR
            EditorSceneManager.sceneSaving -= OnSceneSave;
            #endif
        }
        
        #if UNITY_EDITOR
        private void OnSceneSave(Scene scene, string path) {
        }
        #endif
    }
}