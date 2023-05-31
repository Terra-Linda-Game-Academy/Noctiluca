using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Levels {
	public class LevelController : MonoBehaviour {
		public int spawnNum;

		public int maxHallwayLength = 5;

		public GameObject roomPrefab;
		public GameObject hallwayPrefab; //todo: procedurally generate hallway meshes?

		public RoomPool normalRooms;
		public RoomPool treasureRooms;
		public RoomPool starterRooms;
		public RoomPool bossRooms;

		private List<RoomController>        _rooms;
		private RandomPool<RoomController>  _roomPool;
		private List<(RoomController, int)> _openConnections;

		public void Generate() {
			DateTime start = DateTime.Now;

			ClearChildren();

			_rooms           = new List<RoomController>();
			_openConnections = new List<(RoomController, int)>();

			var possibleStartRoom = starterRooms.One();
			if (!possibleStartRoom.Enabled) {
				Debug.LogError($"Level {name} couldn't find a valid starting room!");
				return;
			}

			// ReSharper disable once UnusedVariable
			RoomController startingRoom = SpawnRoom(possibleStartRoom.Value, transform.position, "Starting Room");

			//IterativeBranching(startingRoom, spawnNum);
			RandomGrowth(spawnNum);

			foreach (RoomController rc in _rooms) { rc.GenerateTerrainMesh(); }
			
			Debug.Log($"Generation took {DateTime.Now.Subtract(start).Milliseconds} ms");
		}

		private void RandomGrowth(int numRooms) {
			for (int i = 0; i < numRooms; i++) {
				bool roomSpawned = false;

				while (_openConnections.Count > 0) {
					int index = Random.Range(0, _openConnections.Count);

					var conn = _openConnections[index];

					if (SpawnBranch(conn.Item1, conn.Item2).Enabled) {
						roomSpawned = true;
						break;
					}

					_openConnections.RemoveAt(index);
				}

				if (!roomSpawned) {
					Debug.LogWarning("Ran out of valid connections in the level, ending generation");
					return;
				}
			}
		}

		// ReSharper disable once UnusedMember.Local
		private void IterativeBranching(RoomController root, int depth) {
			if (depth <= 0) return;

			for (int i = 0; i < root.connections.Length; i++) {
				if (root.connections[i] || Random.value < 0.3) continue;
				var possibleNewRoom = SpawnBranch(root, i);
				if (!possibleNewRoom.Enabled) return;

				IterativeBranching(possibleNewRoom.Value, depth - 1);
			}
		}

		private Option<RoomController> SpawnBranch(RoomController root, int connIndex) {
			bool spawningNormalRoom = Random.value <= 0.8; //todo: change this

			int hallwaySectionLength = 5; //todo: tweak this? same length as prefab

			Room.ConnectionPoint rootConnection = root.Room.connectionPoints[connIndex];

			bool ConnCheck(Room.ConnectionPoint conn) =>
				conn.direction == rootConnection.InverseDirection && !root.connections[connIndex];

			var possibleNewRoom = spawningNormalRoom
				                      ? normalRooms.Where(room => room.connectionPoints.Any(ConnCheck)).One()
				                      : treasureRooms.Where(room => room.connectionPoints.Any(ConnCheck)).One();

			if (!possibleNewRoom.Enabled) {
				Debug.LogError(
					$"Couldn't find a valid room to spawn {rootConnection.direction} of {root.gameObject.name}");
				return Option<RoomController>.None();
			}

			Room newRoom = possibleNewRoom.Value;

			var possibleNewRoomConnection = newRoom.connectionPoints.Where(ConnCheck).One();

			if (!possibleNewRoomConnection.Enabled) {
				Debug.LogError("Newly generated room had no valid connections!");
				return Option<RoomController>.None();
			}

			Room.ConnectionPoint
				newRoomConnection = possibleNewRoomConnection.Value;

			Vector3 rootPos = root.gameObject.transform.position;

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
			for (; numHallwaySegments <= maxHallwayLength; numHallwaySegments++) {
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

			if (!foundRoomPlacement) return Option<RoomController>.None();

			RoomController newRoomController = SpawnRoom(newRoom, proposedNewRoomPos);

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

				GameObject newHallwayObj = Instantiate(hallwayPrefab, root.transform);
				newHallwayObj.name               = $"{root.Room.name} {rootConnection.direction} Hallway {i + 1}";
				newHallwayObj.transform.position = newHallwayPos;
				newHallwayObj.transform.rotation = (int) rootConnection.direction % 2 == 0
					                                   ? Quaternion.Euler(0, 90, 0)
					                                   : Quaternion.identity;
			}

			root.connections[connIndex] = true;
			_openConnections.Remove((root, connIndex));

			int newRoomConnIndex = newRoomController.Room.connectionPoints.IndexOf(newRoomConnection);
			newRoomController.connections[newRoomConnIndex] = true;
			_openConnections.Remove((newRoomController, newRoomConnIndex));

			return Option<RoomController>.Some(newRoomController);
		}

		//todo: find smth better than looping every room every time
		private bool ValidRoomPlacement(Room room, Vector3 pos) {
			foreach (RoomController other in _rooms) {
				/*if (other.Room.Equals(room)) {
					Debug.Log("same room");
					continue;
				}*/ //todo: idk why this needs to be commented out but it works -_-

				if (other.Intersect(room, pos)) return false;
			}

			return true;
		}

		private RoomController SpawnRoom(Room room, Vector3 position, string objName = "") {
			GameObject obj = Instantiate(roomPrefab, transform);
			obj.transform.position = position;

			RoomController controller = obj.GetComponent<RoomController>();
			controller.Room = room;
			controller.Init();

			if (objName == "") { obj.name = room.name + $" {controller.RoomId.ToString()}"; } else {
				obj.name = objName;
			}

			_rooms.Add(controller);
			for (int i = 0; i < controller.connections.Length; i++) { _openConnections.Add((controller, i)); }

			return controller;
		}

		private void ClearChildren() {
			int childCount = transform.childCount;

			for (int i = childCount - 1; i >= 0; i--) { DestroyImmediate(transform.GetChild(i).gameObject); }
		}
	}
}