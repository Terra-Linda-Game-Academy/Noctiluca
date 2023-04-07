using UnityEngine;

namespace Levels {
	public class LevelController : MonoBehaviour {
		public GameObject roomPrefab;
		
		public RoomPool normalRooms;
		public RoomPool treasureRooms;
		public RoomPool starterRooms;
		public RoomPool bossRooms;

		public int radius = 5;

		private void Start() => Generate();

		private void Generate() {
			//spawn starting room
			SpawnRoom(starterRooms, Vector3.zero, "Starting Room");
		}

		private void SpawnRoom(RoomPool pool, Vector3 position, string objName) {
			Room room = pool.PickOneRoom();
			GameObject obj = Instantiate(roomPrefab, transform);
			obj.name               = objName;
			obj.transform.position = position;

			RoomController controller = obj.GetComponent<RoomController>();
			controller.Room = room;
			controller.Init();
		}
	}
}