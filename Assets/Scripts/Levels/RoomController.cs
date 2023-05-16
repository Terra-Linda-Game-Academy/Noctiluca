using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Color = UnityEngine.Color;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Levels {
	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
	public class RoomController : MonoBehaviour {
		[SerializeField] private Room room;

		public Room Room {
			get => room;
			set => room = value;
		}

		/*[HideInInspector] */public bool[] connections;

		public Guid RoomId { get; private set; }

		public bool Intersect(RoomController other) => Intersect(other.Room, other.transform.position);

		public bool Intersect(Room other, Vector3 otherPos) {
			Vector3 pos       = transform.position;
			float   minX      = pos.x;
			float   minZ      = pos.z;
			float   maxX      = minX + Room.Size.x;
			float   maxZ      = minZ + Room.Size.z;
			
			float   otherMinX = otherPos.x;
			float   otherMinZ = otherPos.z;
			float   otherMaxX = otherMinX + other.Size.x;
			float   otherMaxZ = otherMinZ + other.Size.z;
			
			return minX <= otherMaxX && maxX >= otherMinX && minZ <= otherMaxZ && maxZ >= otherMinZ;
		}

		private MeshRenderer meshRenderer;

		private bool _initted;
		
		private void OnDrawGizmos() {
			
			Vector3 localScale = transform.localScale;

			for (int i = 0; i < connections.Length; i++) {
				Room.ConnectionPoint connection = room.connectionPoints[i];
				bool connected = connections[i];
				
				Vector3 pos;
				Vector3 scale;

				Gizmos.color = connected ? Color.red : Color.green;
				
				switch (connection.direction) {
					case Room.Direction.North:
						pos = new Vector3((connection.coordinate + .5f) * localScale.x, 0,
						                  room.Size.z                   * localScale.z);
						scale = new Vector3(1, 2, .5f);
						break;
					case Room.Direction.East:
						pos = new Vector3(room.Size.x                   * localScale.x, 0,
						                  (connection.coordinate + .5f) * localScale.z);
						scale = new Vector3(.5f, 2, 1);
						break;
					case Room.Direction.South:
						pos   = new Vector3((connection.coordinate + .5f) * localScale.x, 0, 0);
						scale = new Vector3(1, 2, .5f);
						break;
					case Room.Direction.West:
						pos   = new Vector3(0, 0, (connection.coordinate + .5f) * localScale.z);
						scale = new Vector3(.5f, 2, 1);
						break;
					default:
						pos   = Vector3.zero;
						scale = Vector3.one;
						break;
				}

				pos += transform.position + Vector3.up;

				Gizmos.DrawWireCube(pos, scale);
			}
		}

		private void Awake() {
			if (!_initted && room != null) Init();
		}

		public void Init() {
			#if UNITY_EDITOR
			EditorSceneManager.sceneSaving += OnSceneSave;
			#endif
			RoomId       = new Guid();
			meshRenderer = GetComponent<MeshRenderer>();
			
			connections ??= new bool[room.connectionPoints.Count];

			foreach (var tile in room.TileAssets) {
				if (!tile.CreateGameObject) return;
				GameObject obj          = new GameObject(tile.Name);
				Transform  objTransform = obj.transform;
				objTransform.parent = transform;
				int x = tile.Position.x, z = tile.Position.y;
				objTransform.position = new Vector3(x, room.GetTileAt(x, z).Height, z);
				tile.Init(obj, RoomId, Room);
			}

			foreach (var tile in room.Tiles) {
				GameObject obj          = new GameObject(tile.Name);
				Transform  objTransform = obj.transform;
				objTransform.parent = transform;
				int x = tile.Position.x, z = tile.Position.y;
				objTransform.position = new Vector3(x, room.GetTileAt(x, z).Height, z);
				tile.Init(obj, RoomId, Room);
			}

			GenerateTerrainMesh();

			_initted = true;
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

		//private static readonly int tileMapShaderId = Shader.PropertyToID("TileMap");

		public void GenerateTerrainMesh() {
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

			Mesh mesh = TerrainMesh.Generate(room, connections);

			GetComponent<MeshFilter>().mesh         = mesh;
			GetComponent<MeshCollider>().sharedMesh = mesh;
		}
	}
}