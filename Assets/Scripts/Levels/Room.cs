using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Levels {
	[CreateAssetMenu(fileName = "Room", menuName = "Room", order = 0)]
	public class Room : ScriptableObject {
		[Serializable, Flags]
		public enum TileFlags : ushort {
			Wall = 0b0000000000000001,
			Pit  = 0b0000000000000010,
			None = 0b0000000000000100
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

		[SerializeField] private Vector3Int size;

		public Vector3Int Size => size;

		/// <Summary> lower half </Summary>
		[SerializeField] public Tile[] tileMap;

		[SerializeField]     private TileAsset[]  tileAssets;
		[SerializeReference] private SimpleTile[] tiles;

		public Tile GetTileAt(int x, int z) {
			if (tileMap.Length <= 0) ResetTiles();

			if (x < 0 || x >= size.x || z < 0 || z >= size.z) { //todo: remove this
				return new Tile(TileFlags.Wall, 0);
			}

			/*float height = 0;
			if (z > 6) height = 1;
			if (x > 10) height = 2;
			height += 0.2f * Mathf.Sin(x) * Mathf.Sin(z);
			if (x < 0 || z < 0 || x >= size.x || z >= size.z) { //todo: remove this
			    return new Tile(TileFlags.Wall, Mathf.Clamp(height, 0, size.y));
			}
			return new Tile(height >= size.y ? TileFlags.Wall : TileFlags.None, Mathf.Clamp(height, 0, size.y));*/
			int linearIndex = z * size.x + x;
			return tileMap[linearIndex];
		}

		public bool SetTileAt(Tile tile, int x, int z) {
			//if (tileMap.Length <= 0) ResetTiles();

			if (x < 0 || x >= size.x || z < 0 || z >= size.z) { return false; }

			int linearIndex = z * size.x + x;
			tileMap[linearIndex] = tile;
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
					int oldLinearIndex = GetLinearIndex(size, i, j);
					int newLinearIndex = GetLinearIndex(newSize, i, j);
					
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

		private int GetLinearIndex(Vector3Int roomSize, int x, int y) {
			return y * roomSize.x + x;
		}
	}
}