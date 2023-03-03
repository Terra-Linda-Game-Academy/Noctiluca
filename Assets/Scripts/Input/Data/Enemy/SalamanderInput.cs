using AI;
using UnityEngine;

namespace Input.Data.Enemy
{
    public struct SalamanderInput : IInputBlockable
    {
        public Vector3 TargetPos;
        public Vector3 PlayerPos;
        public float PlayerSpeed;

        public bool looking;

        public float Speed;

        public SalamanderState State;
        public void Block()
        {
            TargetPos = Vector2.zero;
        }
    }

}
