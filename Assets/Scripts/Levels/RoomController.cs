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
        [SerializeField] private Room room;
        
        public Guid RoomId { get; private set; }
        
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        private void Awake() {
            #if UNITY_EDITOR 
            EditorSceneManager.sceneSaving += OnSceneSave;
            #endif
            RoomId = new Guid();
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            
            foreach (var tile in room.TileAssets) {
                if (!tile.CreateGameObject) return;
                GameObject obj = new GameObject(tile.Name);
                Transform objTransform = obj.transform;
                objTransform.parent = transform;
                objTransform.position = tile.Position;
                tile.Init(obj, RoomId);
            }

            foreach (var tile in room.Tiles) {
                GameObject obj = new GameObject(tile.Name);
                Transform objTransform = obj.transform;
                objTransform.parent = transform;
                objTransform.position = tile.Position;
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
            //todo: save room asset here if necessary
        }
        #endif
    }
}