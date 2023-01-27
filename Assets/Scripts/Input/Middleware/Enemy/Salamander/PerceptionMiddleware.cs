using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;
using Util;

namespace Input.Middleware.Enemy.Salamander
{
    public class PerceptionMiddleware : InputMiddleware<SalamanderInput, SalamanderInputEvents.Dispatcher>
    {
        public float MaxViewDistance = 20.0f;
        public float MaxViewAngle = 60.0f;

        public ScriptableVar<MonoBehaviour> Player;

        public override void TransformInput(ref SalamanderInput inputData)
        {

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