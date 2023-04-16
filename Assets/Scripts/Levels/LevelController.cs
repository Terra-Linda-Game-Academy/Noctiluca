using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Util.ConcretePools;
using Random = UnityEngine.Random;

namespace Levels {
	public class LevelController : MonoBehaviour {
		public GameObject roomPrefab;
		public GameObject hallwayPrefab; //todo: procedurally generate hallway meshes?

		public RoomPool normalRooms;
		public RoomPool treasureRooms;
		public RoomPool starterRooms;
		public RoomPool bossRooms;

		public int radius = 5;

		private const int MaxHallwayLength = 5;

		private List<RoomObj> _rooms;

		private struct RoomObj : IEquatable<RoomObj> {
			public Room       Room;
			public GameObject Obj;

			public RoomObj(Room room, GameObject obj) {
				Room = room;
				Obj  = obj;
			}

			public bool Intersect(Room room, Vector3 pos) {
				float minX = Obj.transform.position.x;
				float minZ = Obj.transform.position.z;
				float maxX = minX + Room.Size.x;
				float maxZ = minZ + Room.Size.z;

				float otherMinX = pos.x;
				float otherMinZ = pos.z;
				float otherMaxX = otherMinX + room.Size.x;
				float otherMaxZ = otherMinZ + room.Size.z;

				return minX <= otherMaxX && maxX >= otherMinX && minZ <= otherMaxZ && maxZ >= otherMinZ;
			}

			public bool Equals(RoomObj other) {
				return Equals(Room, other.Room)
				    && Equals(Obj, other.Obj);
			}

			public override bool Equals(object obj) { return obj is RoomObj other && Equals(other); }

			public override int GetHashCode() { return HashCode.Combine(Room, Obj); }
		}

		private void Start() => Generate();

		private void Generate() {
			_rooms = new List<RoomObj>();

			var possibleStartRoom = starterRooms.One();
			if (!possibleStartRoom.Enabled) {
				Debug.LogError($"Level {name} couldn't find a valid starting room!");
				return;
			}
			
			RoomObj startingRoom = SpawnRoom(possibleStartRoom.Value, transform.position, "Starting Room");

			IterativeBranching(startingRoom, 2);
		}

		private void IterativeBranching(RoomObj root, int depth) {
			if (depth <= 0) return;

			foreach (Room.ConnectionPoint conn in root.Room.connections.All()) {
				if (conn.connected) continue;
				var possibleNewRoom = SpawnBranch(root, conn);
				if (!possibleNewRoom.Enabled) return;

				IterativeBranching(possibleNewRoom.Value, depth - 1);
			}
		}

		private Option<RoomObj> SpawnBranch(RoomObj root, Room.ConnectionPoint rootConnection) {
			bool spawningNormalRoom = Random.value >= 0.5; //todo: change this

			int hallwaySectionLength = 5; //todo: tweak this? same length as prefab

			bool ConnCheck(Room.ConnectionPoint conn) =>
				conn.direction == rootConnection.InverseDirection && !conn.connected;

			var possibleNewRoom = spawningNormalRoom
				                       ? normalRooms.One(room => room.connections.HasAny(ConnCheck))
				                       : treasureRooms.One(room => room.connections.HasAny(ConnCheck));

			if (!possibleNewRoom.Enabled) {
				Debug.LogError($"Couldn't find a valid room to spawn {rootConnection.direction} of {root.Room.name}");
				return Option<RoomObj>.None();
			}

			Room newRoom = possibleNewRoom.Value;
			
			var possibleNewRoomConnection = newRoom.connections.One(ConnCheck);

			if (!possibleNewRoomConnection.Enabled) {
				Debug.LogError("Newly generated room had no valid connections!");
				return Option<RoomObj>.None();
			}

			Room.ConnectionPoint
				newRoomConnection = possibleNewRoomConnection.Value;

			Vector3 rootPos = root.Obj.transform.position;

			Vector3 proposedNewRoomPos = rootConnection.direction switch {
				                             Room.Direction.North => rootPos
				                                                   + new Vector3(
					                                                     rootConnection.coordinate
					                                                   - newRoomConnection.coordinate,
					                                                     0,
					                                                     root.Room.Size.z + hallwaySectionLength),
				                             Room.Direction.East => rootPos
				                                                  + new Vector3(
					                                                    root.Room.Size.x + hallwaySectionLength,
					                                                    0,
					                                                    rootConnection.coordinate
					                                                  - newRoomConnection.coordinate),
				                             Room.Direction.South => rootPos
				                                                   + new Vector3(
					                                                     rootConnection.coordinate
					                                                   - newRoomConnection.coordinate,
					                                                     0,
					                                                     -(newRoom.Size.z + hallwaySectionLength)),
				                             Room.Direction.West => rootPos
				                                                  + new Vector3(
					                                                    -(newRoom.Size.x + hallwaySectionLength),
					                                                    0,
					                                                    rootConnection.coordinate
					                                                  - newRoomConnection.coordinate),
				                             _ => Vector3.zero
			                             };

			bool foundRoomPlacement = false;
			int  numHallwaySegments = 1;
			for (; numHallwaySegments <= MaxHallwayLength; numHallwaySegments++) {
				if (ValidRoomPlacement(newRoom, proposedNewRoomPos)) {
					foundRoomPlacement = true;
					break;
				}

				proposedNewRoomPos += rootConnection.direction switch {
					                      Room.Direction.North => new Vector3(0, 0, hallwaySectionLength),
					                      Room.Direction.East  => new Vector3(hallwaySectionLength, 0, 0),
					                      Room.Direction.South => new Vector3(0, 0, -hallwaySectionLength),
					                      Room.Direction.West  => new Vector3(-hallwaySectionLength, 0, 0),
					                      _                    => Vector3.zero
				                      };
			}

			if (!foundRoomPlacement)
				Debug.LogWarning(
					$"Could not find a valid room placement {rootConnection.direction} of {root.Room.name}, there may be some room clipping.");

			RoomObj newRoomObj = SpawnRoom(newRoom, proposedNewRoomPos, newRoom.name);

			for (int i = 0; i < numHallwaySegments; i++) {
				Vector3 newHallwayPos = rootConnection.direction switch {
					                        Room.Direction.North => rootPos
					                                              + new Vector3(
						                                                rootConnection.coordinate + .5f, 0,
						                                                root.Room.Size.z
						                                              + hallwaySectionLength / 2f
						                                              + i                    * hallwaySectionLength),
					                        Room.Direction.East => rootPos
					                                             + new Vector3(
						                                               root.Room.Size.x
						                                             + hallwaySectionLength / 2f
						                                             + i                    * hallwaySectionLength,
						                                               0,
						                                               rootConnection.coordinate + .5f),
					                        Room.Direction.South => rootPos
					                                              + new Vector3(
						                                                rootConnection.coordinate + .5f, 0,
						                                                -(hallwaySectionLength / 2f
						                                                + i                    * hallwaySectionLength)),
					                        Room.Direction.West => rootPos
					                                             + new Vector3(
						                                               -(hallwaySectionLength / 2f
						                                               + i * hallwaySectionLength), 0,
						                                               rootConnection.coordinate + .5f),
					                        _ => Vector3.zero
				                        };

				GameObject newHallwayObj = Instantiate(hallwayPrefab, root.Obj.transform);
				newHallwayObj.name               = $"{root.Room.name} {rootConnection.direction} Hallway {i + 1}";
				newHallwayObj.transform.position = newHallwayPos;
				newHallwayObj.transform.rotation = (int) rootConnection.direction % 2 == 0
					                                   ? Quaternion.Euler(0, 90, 0)
					                                   : Quaternion.identity;
			}

			rootConnection.connected    = true;
			newRoomConnection.connected = true;

			return Option<RoomObj>.Some(newRoomObj);
		}

		//todo: find smth better than looping every room every time
		private bool ValidRoomPlacement(Room room, Vector3 pos) {
			foreach (RoomObj other in _rooms) {
				/*if (other.Room.Equals(room)) {
					Debug.Log("same room");
					continue;
				}*/ //todo: idk why this needs to be commented out but it works -_-

				if (other.Intersect(room, pos)) return false;
			}

			return true;
		}

		private RoomObj SpawnRoom(Room room, Vector3 position, string objName) {
			GameObject obj = Instantiate(roomPrefab, transform);
			obj.name               = objName;
			obj.transform.position = position;

			RoomController controller = obj.GetComponent<RoomController>();
			controller.Room = room;
			controller.Init();

			RoomObj newRoom = new RoomObj(room, obj);

			_rooms.Add(newRoom);

			return newRoom;
		}
	}
}