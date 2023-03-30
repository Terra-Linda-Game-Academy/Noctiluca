using UnityEngine;

namespace Levels {
	public class LevelController : MonoBehaviour {
		public RoomPool normalRooms;
		public RoomPool treasureRooms;
		public RoomPool starterRooms;
		public RoomPool bossRooms;

		private void Start() => Generate();

		private void Generate() {
			
		}
	}
}