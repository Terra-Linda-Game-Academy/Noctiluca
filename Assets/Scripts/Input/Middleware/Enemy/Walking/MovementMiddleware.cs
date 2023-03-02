using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.Middleware.Enemy.Walking
{
    public class
        MovementMiddleware : InputMiddleware<WalkingEnemyInput, WalkingEnemyInputEvents.Dispatcher>
    {
        public float MinDistance = 1.0f;

        public override void TransformInput(ref WalkingEnemyInput inputData)
        {
            switch (inputData.State)
            {
                case WalkingEnemyState.Chase:
                    Vector3 toPlayer = inputData.PlayerPos - perceptron.transform.position;
                    inputData.Movement = toPlayer.magnitude > MinDistance
                                             ? new Vector2(toPlayer.x, toPlayer.z).normalized
                                             : Vector2.zero;

                    break;
                case WalkingEnemyState.Idle:
                    inputData.Movement = Vector2.zero;
                    break;
                default:
                    inputData.Movement = Vector2.zero;
                    break;
            }
        }

        public override void Init() { }

        public override InputMiddleware<WalkingEnemyInput, WalkingEnemyInputEvents.Dispatcher> Clone()
        {
            return new MovementMiddleware { MinDistance = MinDistance };
        }
    }
}