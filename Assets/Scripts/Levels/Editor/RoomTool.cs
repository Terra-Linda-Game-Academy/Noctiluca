using System;
using System.Linq;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Levels.Editor {
	[EditorTool("Room Tool", typeof(RoomController))]
	public class RoomTool : EditorTool, IDrawSelectedHandles {
		public override GUIContent toolbarIcon => new GUIContent("Room Tool", "Use this to edit room objects");

		private RoomController _controller;
		private Room           Room => _controller.Room;

		private Type[] _simpleTileTypes;
		private Type[] _assetTileTypes;
		private Rect   _typeWindow;

		private void OnEnable() {
			_controller = (RoomController) target;

			//_simpleTileTypes  = AssignTileTypes().ToArray();
			AssignTileTypes();
			
			_typeWindow = new Rect(50, 20, Mathf.Max(GetTypeWindowWidth(), 40f), 80);

			SetMeshes();
		}

		private void SetMeshes() {
			Mesh mesh = TerrainMesh.Generate(_controller.Room);

			_controller.GetComponent<MeshFilter>().sharedMesh   = mesh;
			_controller.GetComponent<MeshCollider>().sharedMesh = mesh;
		}

		public override void OnToolGUI(EditorWindow window) {
			if (Room is null) return;

			DrawGUI(window);

			Vector3Int sizeVal = Room.Size;
			Vector3Int newSize = new Vector3Int();

			using (new Handles.DrawingScope(_controller.transform.localToWorldMatrix)) {
				Handles.color = Handles.xAxisColor;
				var xHandlePos = new Vector3(sizeVal.x, 0, 0);
				newSize.x = Mathf.FloorToInt(Mathf.Max(1, Handles.ScaleSlider(
					                                       sizeVal.x, xHandlePos, Vector3.right,
					                                       Quaternion.identity, HandleUtility.GetHandleSize(xHandlePos),
					                                       1
				                                       )));

				Handles.color = Handles.yAxisColor;
				var yHandlePos = new Vector3(0, sizeVal.y, 0);
				newSize.y = Mathf.FloorToInt(Mathf.Max(1, Handles.ScaleSlider(
					                                       sizeVal.y, yHandlePos, Vector3.up,
					                                       Quaternion.identity, HandleUtility.GetHandleSize(yHandlePos),
					                                       1
				                                       )));

				Handles.color = Handles.zAxisColor;
				var zHandlePos = new Vector3(0, 0, sizeVal.z);
				newSize.z = Mathf.FloorToInt(Mathf.Max(1, Handles.ScaleSlider(
					                                       sizeVal.z, zHandlePos, Vector3.forward,
					                                       Quaternion.identity, HandleUtility.GetHandleSize(zHandlePos),
					                                       1
				                                       )));
			}

			if (newSize != Room.Size) {
				Undo.RecordObject(Room, $"Resizing {Room.name}");
				Room.UpdateSize(newSize);
				SetMeshes();
				EditorUtility.SetDirty(Room);
			}

			//todo: setup Editor3D and Property3D stuff, and call draw methods for each
		}


		public void OnDrawHandles() {
			using (new Handles.DrawingScope(Color.white, _controller.transform.localToWorldMatrix)) {
				if (Room is null) {
					using (new Handles.DrawingScope(Color.red)) {
						Vector3 half = new Vector3(0.5f, 0.5f, 0.5f);
						Handles.DrawWireCube(half, Vector3.one);
						Handles.DrawWireCube(half, half);
					}

					return;
				}

				Handles.DrawWireCube((Vector3) Room.Size / 2.0f, Room.Size);

				Handles.color = Color.yellow;
				//todo: draw all preview things of room tiles
				for (int x = 0; x < Room.Size.x; x++) {
					for (int z = 0; z < Room.Size.z; z++) {
						Room.Tile tile = Room.GetTileAt(x, z);
						if (tile.flags != Room.TileFlags.None) continue;

						float height = tile.Height;

						Handles.DrawPolyLine(
							new Vector3(x, height, z),
							new Vector3(x, height, z + 1),
							new Vector3(x            + 1, height, z + 1),
							new Vector3(x            + 1, height, z),
							new Vector3(x, height, z)
						);
					}
				}
			}
		}

		private void DrawGUI(EditorWindow window) {
			Handles.BeginGUI();

			_typeWindow = GUI.Window(0, _typeWindow, DrawTileTypesWindow, "Tile Entities");

			if (GUI.Button(new Rect(0, window.position.height - 30 - 30, 100, 30),
			               "Reset Mesh")) { SetMeshes(); }

			Handles.EndGUI();
		}

		private void DrawTileTypesWindow(int _) {
			float buttonX = 10;
			
			foreach (Type tileType in _simpleTileTypes) {
				string typeName = tileType.Name;
				
				if (GUI.Button(new Rect(buttonX, 30, GetButtonWidth(typeName.Length), 20), tileType.Name)) {
					Debug.Log($"clicked {tileType.Name}");
				}

				buttonX += GetButtonWidth(typeName.Length) + 10;
			}
			
			foreach (Type tileType in _assetTileTypes) {
				string typeName = tileType.Name;
				
				if (GUI.Button(new Rect(buttonX, 30, GetButtonWidth(typeName.Length), 20), tileType.Name)) {
					Debug.Log($"clicked {tileType.Name}");
				}

				buttonX += GetButtonWidth(typeName.Length) + 10;
			}

			GUI.DragWindow();
		}

		private void AssignTileTypes() {
			_simpleTileTypes = TypeCache.GetTypesDerivedFrom<SimpleTile>().ToArray();
			_assetTileTypes  = TypeCache.GetTypesDerivedFrom<TileAsset>().ToArray();
		}

		private static float GetButtonWidth(int charCount) {
			return charCount * 8f;
		}

		private float GetTypeWindowWidth() {
			float width = 10 * 1;

			foreach (Type type in _simpleTileTypes) {
				width += GetButtonWidth(type.Name.Length) + 10;
			}
			
			foreach (Type type in _assetTileTypes) {
				width += GetButtonWidth(type.Name.Length) + 10;
			}

			return width;
		}
	}
}