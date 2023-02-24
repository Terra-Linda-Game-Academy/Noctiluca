using UnityEngine;

namespace Levels {
	public class StupidRoomMeshGen : MonoBehaviour {
		private Mesh GenerateMesh() {
			Mesh mesh = new Mesh {name = "Dumb Stupid Mesh"};

			RoomController roomController = GetComponent<RoomController>();

			int xScale = roomController.Room.Size.x;
			int zScale = roomController.Room.Size.z;

			mesh.vertices = new[] {
				                      Vector3.zero,
				                      Vector3.forward * zScale,
				                      Vector3.forward * zScale + Vector3.right * xScale,
				                      Vector3.right * xScale
			                      };

			mesh.triangles = new[] {0, 1, 2, 0, 2, 3};

			return mesh;
		}

		public void AdjustMesh() { GetComponent<MeshFilter>().sharedMesh = GenerateMesh(); }
	}
}