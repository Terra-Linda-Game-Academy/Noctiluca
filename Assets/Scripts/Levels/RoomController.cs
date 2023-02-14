using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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

        //private void OnEnable() => StartCoroutine(GenerateTerrainMesh());

        private void SetupTerrainRenderer() {
            meshRenderer.sharedMaterials = new Material[] { }; //todo: grab materials from Terrain
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
        
        
        
        private static readonly int tileMapShaderId = Shader.PropertyToID("TileMap");

        private void GenerateTerrainMesh() {
            /*ComputeShader computeTerrain = Resources.Load<ComputeShader>("GenerateTerrain");
            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            
            int dataKernel = computeTerrain.FindKernel("GenerateData"),
                meshKernel = computeTerrain.FindKernel("GenerateMesh");
            
            int roomArea = room.Size.x * room.Size.z;

            ComputeBuffer tileBuffer = new ComputeBuffer(roomArea, Room.Tile.Stride, ComputeBufferType.Raw);
            tileBuffer.SetData(room.tileMap);
            computeTerrain.SetBuffer(dataKernel, tileMapShaderId, tileBuffer);
            computeTerrain.SetBuffer(meshKernel, tileMapShaderId, tileBuffer);

            int vertexCount = 0; //todo: set these
            int indexCount = 0;

            //todo: check if target format is right??????
            GraphicsBuffer vertexBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Vertex, vertexCount, Vertex.Stride); //todo: get actual vert count
            GraphicsBuffer indexBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Index, indexCount, sizeof(uint));
            //todo: set buffers on GPU, and allocate sum buffers for vertices and indices

            var cmd = CommandBufferPool.Get("Generate Terrain");
            cmd.DispatchCompute(computeTerrain, dataKernel, 1, 1, 1); //todo: not right, get next multiple of thread group size
            var dataDone = cmd.CreateAsyncGraphicsFence();
            cmd.WaitOnAsyncGraphicsFence(dataDone);
            cmd.DispatchCompute(computeTerrain, meshKernel, 1, 1, 1); //todo: use indirect args buffer
            var meshDone = cmd.CreateAsyncGraphicsFence();
            Graphics.ExecuteCommandBufferAsync(cmd, ComputeQueueType.Background);
            CommandBufferPool.Release(cmd);
            yield return new WaitUntil(() => meshDone.passed);

            NativeArray<Vertex> generatedVertices =
                new NativeArray<Vertex>(vertexCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            NativeArray<ushort> generatedIndices =
                new NativeArray<ushort>(indexCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            var vertexReadback = AsyncGPUReadback.RequestIntoNativeArray(ref generatedVertices, vertexBuffer);
            var indexReadback = AsyncGPUReadback.RequestIntoNativeArray(ref generatedIndices, indexBuffer);
            
            yield return new WaitUntil(() => vertexReadback.done && indexReadback.done);

            //todo: reduce data size
            mesh.SetVertexBufferParams(vertexCount,
                new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2)
            );
            mesh.SetVertexBufferData(generatedVertices, 0, 0, vertexCount, 0, MeshUpdateFlags.Default);
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
            mesh.SetIndexBufferData(generatedIndices, 0, 0, indexCount, MeshUpdateFlags.Default);*/
            
            //--------------------------------------------------------------------------------------------------------//
            
            /*ComputeShader computeTerrain = Resources.Load<ComputeShader>("GenerateTerrain");
            
            int meshKernel = computeTerrain.FindKernel("GenerateMesh");
            int roomArea = room.Size.x * room.Size.z;
            
            var cmd = CommandBufferPool.Get("Generate Terrain");
            //cmd.SetExecutionFlags(CommandBufferExecutionFlags.AsyncCompute);

            /*ComputeBuffer tileBuffer = new ComputeBuffer(roomArea, Room.Tile.Stride, ComputeBufferType.Raw);
            tileBuffer.SetData(room.tileMap);
            cmd.SetComputeBufferParam(computeTerrain, meshKernel, tileMapShaderId, tileBuffer);#1#

            int vertexCount = roomArea * 4; //todo: set these
            int indexCount = roomArea * 6;

            //todo: check if target format is right??????
            GraphicsBuffer vertexBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Raw | GraphicsBuffer.Target.Vertex, vertexCount * 8, sizeof(uint)); //todo: get actual vert count
            GraphicsBuffer indexBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Raw | GraphicsBuffer.Target.Index, indexCount, sizeof(uint));
            cmd.SetComputeBufferParam(computeTerrain, meshKernel, "GeneratedVertices", vertexBuffer); //todo: use nameID
            cmd.SetComputeBufferParam(computeTerrain, meshKernel, "GeneratedIndices", indexBuffer);
            cmd.SetComputeIntParams(computeTerrain, "RoomDimensions", room.Size.x, room.Size.y, room.Size.z);
            cmd.DispatchCompute(computeTerrain, meshKernel, 1, 1, 1); //todo: use indirect args buffer
            //todo: set buffers on GPU, and allocate sum buffers for vertices and indices

            //var meshDone = cmd.CreateAsyncGraphicsFence();
            Graphics.ExecuteCommandBuffer/*Async#1#(cmd/*, ComputeQueueType.Background#1#);
            CommandBufferPool.Release(cmd);

            yield return null;
            
            var vertexReadback = AsyncGPUReadback.Request(vertexBuffer);
            var indexReadback = AsyncGPUReadback.Request(indexBuffer);

            yield return new WaitUntil(() => vertexReadback.done && indexReadback.done);

            Mesh mesh = new Mesh();
            mesh.SetVertexBufferParams(vertexCount,
                new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2)
            );
            mesh.SetVertexBufferData(
                vertexReadback.GetData<Vertex>(), 
                0, 0, vertexCount//, 0, 
                //MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds
            );
            
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
            mesh.SetIndexBufferData(
                indexReadback.GetData<uint>(), 
                0, 0, indexCount//, 
                //MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds
            );
            
            mesh.subMeshCount = 1;
            mesh.SetSubMesh(0, new SubMeshDescriptor(0, indexCount));

            //tileBuffer.Release();
            vertexBuffer.Release();
            indexBuffer.Release();
            
            Debug.Log(GetComponent<MeshRenderer>().bounds);
            
            foreach(var vert in mesh.vertices) Debug.Log(vert);
            /*mesh.bounds = new Bounds {
                min = Vector3.zero,
                max = room.Size
            };#1#
            
            //mesh.Optimize();
            GetComponent<MeshFilter>().mesh = mesh;*/
            

            List<Vertex> vertices = new List<Vertex>();
            List<ushort> indices = new List<ushort>();

            //start at -1 because we need to consider the boundaries of the room
            for (int x = -1; x > room.Size.x; x++) {
                for (int z = -1; z > room.Size.z; z++) {
                    //todo: generate each tile here
                    Room.Tile tile0 = room.GetTileAt(x, z);
                    Room.Tile tile1 = room.GetTileAt(x + 1, z + 1);
                    Room.Tile tile2 = room.GetTileAt(x, z);
                    Room.Tile tile3 = room.GetTileAt(x + 1, z + 1);
                    
                    
                }
            }
            /*var mesh = new Mesh();
            
            mesh.SetVertexBufferParams(vertexCount,
                new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2)
            );
            mesh.SetVertexBufferData(
                vertexReadback.GetData<Vertex>(), 
                0, 0, vertexCount//, 0, 
                //MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds
            );
            
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
            mesh.SetIndexBufferData(
                indexReadback.GetData<uint>(), 
                0, 0, indexCount//, 
                //MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds
            );
            
            mesh.subMeshCount = 1;
            mesh.SetSubMesh(0, new SubMeshDescriptor(0, indexCount));
            
            mesh.Optimize();
            GetComponent<MeshFilter>().mesh = mesh;*/

        }
        
        
        
        
    }
}