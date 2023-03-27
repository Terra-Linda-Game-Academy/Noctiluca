using System;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
	[CreateAssetMenu(menuName = "Levels/Room Pool", fileName = "Room Pool")]
	public class RoomPool : ScriptableObject {
		[SerializeField] public List<WrappedRoom> rooms = new List<WrappedRoom>();

		[Serializable]
		public struct WrappedRoom : IEquatable<WrappedRoom> {
			public Room room;
			public int  spawnWeight;
			public bool unique;

			public bool Equals(WrappedRoom other) {
				return Equals(room, other.room)
				    && spawnWeight == other.spawnWeight
				    && unique      == other.unique;
			}

			public override bool Equals(object obj) {
				return obj is WrappedRoom other && Equals(other);
			}

			public override int GetHashCode() {
				return HashCode.Combine(room, spawnWeight, unique);
			}
		}
	}
}