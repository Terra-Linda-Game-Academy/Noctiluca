using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Util.ConcretePools;

namespace Levels {
	[CreateAssetMenu(fileName = "Room", menuName = "Levels/Room", order = 0)]
	public class Room : ScriptableObject {
		[Serializable, Flags]
		public enum TileFlags : ushort {
			Wall = 0b0000000000000001,
			Pit  = 0b0000000000000010,
			None = 0b0000000000000100
		}

		[Serializable]
		public enum Direction : byte {
			North = 0,
			East  = 1,
			South = 2,
			West  = 3
		}

		[Serializable, StructLayout(LayoutKind.Sequential)]
		public struct Tile {
			public const int Stride = sizeof(TileFlags) + sizeof(ushort);

			public TileFlags flags;
			public ushort    packedHeight;

			public float Height => Mathf.HalfToFloat(packedHeight);

			public Tile(TileFlags flags, float height) {
				this.flags   = flags;
				packedHeight = Mathf.FloatToHalf(height);
			}
		}

		[Serializable, StructLayout(LayoutKind.Sequential)]
		public class ConnectionPoint {
			public byte      coordinate;
			public Direction direction;
			public Direction InverseDirection => (Direction) ((int) (direction + 2) % 4);

			[NonSerialized] public bool connected;

			public ConnectionPoint Clone() {
				return new ConnectionPoint {coordinate = coordinate, direction = direction, connected = connected};
			}
		}

		[SerializeField] private Vector3Int size;

		public Vector3Int Size => size;

		/// <Summary> lower half </Summary>
		[SerializeField] public Tile[] tileMap;

		[SerializeField]     private TileAsset[]  tileAssets;
		[SerializeReference] private SimpleTile[] tiles;

		[SerializeField] public ConnectionPool connections;

		public Tile GetTileAt(int x, int z) {
			if (tileMap.Length <= 0) ResetTiles();

			if (x < 0 || x >= size.x || z < 0 || z >= size.z) return new Tile(TileFlags.Wall, 0);

			return tileMap[ToLinearIndex(x, z)];
		}

		public bool SetTileAt(Tile tile, int x, int z) {
			if (x < 0 || x >= size.x || z < 0 || z >= size.z) { return false; }

			tileMap[ToLinearIndex(x, z)] = tile;
			return true;
		}

		public IEnumerable<TileAsset> TileAssets {
			get {
				foreach (var ta in tileAssets) yield return ta;
			}
		}

		public IEnumerable<SimpleTile> Tiles {
			get {
				foreach (var t in tiles) yield return t;
			}
		}

		public IEnumerable<ITile> AllTiles {
			get {
				foreach (var ta in tileAssets) yield return ta;
				foreach (var t in tiles) yield return t;
			}
		}

		public void ResetTiles() {
			tileMap = new Tile[size.x * size.z];

			for (int i = 0; i < size.x; i++) {
				for (int j = 0; j < size.z; j++) { SetTileAt(new Tile(TileFlags.None, 0f), i, j); }
			}
		}

		public void UpdateSize(Vector3Int newSize) {
			Tile[] newArray = new Tile[newSize.x * newSize.z];

			for (int i = 0; i < newSize.x; i++) {
				for (int j = 0; j < newSize.z; j++) {
					int oldLinearIndex = ToLinearIndex(i, j, size);
					int newLinearIndex = ToLinearIndex(i, j, newSize);

					if (i >= size.x || j >= size.z) {
						newArray[newLinearIndex] = new Tile(TileFlags.None, 0f);
						continue;
					}

					newArray[newLinearIndex] = tileMap[oldLinearIndex];
				}
			}

			tileMap = newArray;
			size    = newSize;
		}

		public int ToLinearIndex(int x, int z) => ToLinearIndex(x, z, size);

		public static int ToLinearIndex(int x, int z, Vector3Int specificSize) { return z * specificSize.x + x; }

		public Vector2Int FromLinearIndex(int i) => FromLinearIndex(i, size);

		public static Vector2Int FromLinearIndex(int i, Vector3Int specificSize) {
			int z = Math.DivRem(i, specificSize.x, out int x);
			return new Vector2Int(x, z);
		}

		public Room Clone() {
			Room newRoom = CreateInstance<Room>();
			newRoom.size       = size;
			newRoom.tileMap    = (Tile[]) tileMap.Clone();
			newRoom.tileAssets = (TileAsset[]) tileAssets.Clone();
			newRoom.tiles      = (SimpleTile[]) tiles.Clone();
			newRoom.name       = name;

			newRoom.connections = CreateInstance<ConnectionPool>();
			ConnectionPoint[] oldConns                              = connections.All().ToArray();
			ConnectionPoint[] newConns                              = new ConnectionPoint[connections.Count];
			for (int i = 0; i < connections.Count; i++) newConns[i] = oldConns[i].Clone();
			newRoom.connections.Fill(newConns);

			return newRoom;
		}
	}
}