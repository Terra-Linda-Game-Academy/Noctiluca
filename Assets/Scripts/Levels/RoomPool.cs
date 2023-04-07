using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Levels {
	[CreateAssetMenu(menuName = "Levels/Room Pool", fileName = "Room Pool")]
	public class RoomPool : ScriptableObject /*, IEnumerable<Room>*/ {
		[SerializeReference] public List<WrappedRoom> rooms = new();

		private const int MaxRoomSpawnAttempts = 10;

		private int _roomWeightTotal;

		public Room PickOneRoom() {
			/*if (_roomWeightTotal == 0)*/
			_roomWeightTotal = GetTotalRoomWeight();

			for (int attempts = 0; attempts < MaxRoomSpawnAttempts; attempts++) {
				int randomNumber = Random.Range(0, _roomWeightTotal);

				Debug.Log($"num: {randomNumber}");

				int currentWeight = 0;
				foreach (WrappedRoom wrappedRoom in rooms) {
					currentWeight += wrappedRoom.SpawnWeight;
					if (randomNumber < currentWeight) {
						if (!wrappedRoom.unique) return wrappedRoom.room;

						if (!wrappedRoom.HasBeenUsed) {
							wrappedRoom.HasBeenUsed = true;
							return wrappedRoom.room;
						}

						break;
					}
				}

				Debug.Log($"RoomPool {name} tried to pick an existing unique room, trying again...");
			}

			Debug.LogWarning(
				$"RoomPool {name} tried {MaxRoomSpawnAttempts} times but couldn't find a room, picking the first room.");
			return rooms[0].room;
		}

		private int GetTotalRoomWeight() {
			int total = 0;

			foreach (WrappedRoom wrappedRoom in rooms) {
				Debug.Log($"found room {wrappedRoom.room.name} with weight {wrappedRoom.SpawnWeight}");
				total += wrappedRoom.SpawnWeight;
			}

			Debug.Log($"total weights: {total}");

			return total;
		}

		[Serializable]
		public class WrappedRoom : IEquatable<WrappedRoom> {
			[SerializeField] private int spawnWeight;

			public int SpawnWeight {
				get => spawnWeight > 0 ? spawnWeight : 1;
				set => spawnWeight = value;
			}

			public Room room;
			public bool unique;

			[NonSerialized] public bool HasBeenUsed;

			public bool Equals(WrappedRoom other) {
				return other != null
				    && Equals(room, other.room)
				    && spawnWeight == other.spawnWeight
				    && unique      == other.unique
				    && HasBeenUsed == other.HasBeenUsed;
			}

			public override bool Equals(object obj) { return obj is WrappedRoom other && Equals(other); }

			public override int GetHashCode() { return HashCode.Combine(room, spawnWeight, unique, HasBeenUsed); }
		}

		private class NoValidRoomException : Exception {
			public NoValidRoomException() : base() { }

			public NoValidRoomException(string message) : base(message) { }
		}
	}
}