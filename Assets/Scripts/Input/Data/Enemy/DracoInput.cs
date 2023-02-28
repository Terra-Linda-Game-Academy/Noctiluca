using AI;
using UnityEngine;

namespace Input.Data.Enemy {
	public struct DracoInput {
		public Vector3 Movement;
		public Quaternion LookDir;
		public Vector3 PlayerPos;

		public DracoStates State;
	}
}