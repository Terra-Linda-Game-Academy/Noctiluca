using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;
using Util;

namespace Input.Middleware.Enemy.Salamander
{
    public class PerceptionMiddleware : InputMiddleware<SalamanderInput, SalamanderInputEvents.Dispatcher>
    {
        public float MaxViewDistance = 30.0f;
        public float MaxViewAngle = 240.0f;

        public ScriptableVar<MonoBehaviour> Player;

        public override void TransformInput(ref SalamanderInput inputData)
        {
            inputData.PlayerPos = Player.Value.transform.position;
            if (perceptron.VisionCone(Player.Value.gameObject, MaxViewDistance, MaxViewAngle, ~LayerMask.GetMask("Player")))
            {
                inputData.State = SalamanderState.Attack;
            }
            else
            {
                inputData.State = SalamanderState.Idle;
            }
        }

        public override void Init()
        {

        }

        public override InputMiddleware<SalamanderInput, SalamanderInputEvents.Dispatcher> Clone()
        {
            return new PerceptionMiddleware
            {
                MaxViewDistance = MaxViewDistance,
                MaxViewAngle = MaxViewAngle,
                Player = Player
            };
        }
    }
}