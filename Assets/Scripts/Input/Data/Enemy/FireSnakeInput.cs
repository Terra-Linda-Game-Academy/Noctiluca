using AI;
using UnityEngine;

namespace Input.Data.Enemy
{
	public struct FireSnakeInput : IInputBlockable
	{
		public Vector3 TargetDestination;
		public Vector3 PlayerPos;

		public FireSnakeState State;

		public void Block() { TargetDestination = Vector3.zero; }
	}
}