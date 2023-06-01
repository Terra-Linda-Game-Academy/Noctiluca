using AI;
using UnityEngine;

namespace Input.Data.Enemy
{
	public struct AntInput
	{
		public Vector3 Movement;
		public Quaternion LookDir;
		public Vector3 PlayerPos;

		public AntStates State;
	}
}