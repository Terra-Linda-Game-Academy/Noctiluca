using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Levels {
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class RoomController : MonoBehaviour {
        [SerializeField] private Room room;

        public Room Room => room;
        
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
        
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex {
            public const int Stride = 3 * (3 + 3 + 2) * sizeof(float); 
            
            public Vector3 position;
            public Vector3 normal;
            public Vector2 uv;
        }

        private static readonly ComputeShader shader = Resources.Load<ComputeShader>("GenerateTerrain");

        private static readonly int tileMapShaderId = Shader.PropertyToID("TileMap");

        private IEnumerable GenerateTerrainMesh() {
            int dataKernel = shader.FindKernel("GenerateData"),
                meshKernel = shader.FindKernel("GenerateMesh");
            
            Vector3Int roomSize = room.Size;
            int roomArea = roomSize.x * roomSize.z;

            ComputeBuffer tileBuffer = new ComputeBuffer(roomArea, Room.Tile.Stride, ComputeBufferType.Raw);
            tileBuffer.SetData(room.tileMap);
            shader.SetBuffer(dataKernel, tileMapShaderId, tileBuffer);
            shader.SetBuffer(dataKernel, tileMapShaderId, tileBuffer);

            CommandBuffer cmd = CommandBufferPool.Get("Generate Terrain");
            
            cmd.DispatchCompute(shader, dataKernel, 0, 0, 0); //todo: not right, get next multiple of thread group size
            var fence = cmd.CreateAsyncGraphicsFence();
            cmd.WaitOnAsyncGraphicsFence(fence);
            
            cmd.DispatchCompute(shader, meshKernel, 0, 0, 0); //todo: use indirect args buffer

            Graphics.ExecuteCommandBufferAsync(cmd, ComputeQueueType.Background);
            
            var vertReadback = AsyncGPUReadback.RequestIntoNativeArray() // 
            
            CommandBufferPool.Release(cmd);
            
            /*shader.SetBuffer(dataKernel, "Triangles", triBuffer);
            
            shader.SetVector( "RoomDimensions", new Vector4(roomDimensions.x, roomDimensions.y, roomDimensions.z));            
            shader.SetBuffer(idTerrainKernel, "HeightMap", heightBuffer);

            heightBuffer.SetData(Array.ConvertAll(heightMap, e => (int) e));
            
            shader.Dispatch(
                idTerrainKernel, 
                Mathf.CeilToInt(roomDimensions.x / 16.0f), 
                1,
                Mathf.CeilToInt(roomDimensions.z / 16.0f)
            );
            heightBuffer.Release();

            var dataReq = AsyncGPUReadback.Request(triBuffer);
            dataReq.WaitForCompletion();
            NativeArray<Triangle> tris = dataReq.GetData<Triangle>();
            triBuffer.Release();
            
            
            Vector3[] verts = new Vector3[triCount * 3];
            Vector3[] normals = new Vector3[triCount * 3];
            int[] indices = new int[triCount * 3];

            for (int i = 0; i < triCount; i++) {
                int threeI = i * 3;
                verts[threeI] = tris[i].a.position;
                verts[threeI + 1] = tris[i].b.position;
                verts[threeI + 2] = tris[i].c.position;

                normals[threeI] = tris[i].a.normal;
                normals[threeI + 1] = tris[i].b.normal;
                normals[threeI + 2] = tris[i].c.normal;
                
                indices[threeI] = threeI;
                indices[threeI + 1] = threeI + 1;
                indices[threeI + 2] = threeI + 2;
            }

            Mesh mesh = new Mesh();
            
            mesh.SetVertices(verts);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);

            Debug.Log($"Finished Generating mesh with {triCount} triangles!");*/
            
            return mesh;
        }
    }
}