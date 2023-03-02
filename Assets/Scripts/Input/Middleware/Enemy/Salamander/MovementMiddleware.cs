using AI;
using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Input.Middleware.Enemy.Salamander
{
    public class MovementMiddleware : InputMiddleware<SalamanderInput, SalamanderInputEvents.Dispatcher>
    {
        public float MinDistance = 1.0f;
        public float MaxWander = 5.0f;
        public float MaxWanderPause = 10.0f;
        public float AttackCooldown = 3;
        public float FlippedTime = 6;

        SalamanderState lastState;
        float WaitTime = 0;
        bool Waiting = false;
        bool Looking = false;

        NavMeshAgent agent;

        Vector3 TargetPos;

        bool flipped = false;

        public override void TransformInput(ref SalamanderInput inputData)
        {
            Vector2 MyPos = new Vector2(perceptron.transform.position.x, perceptron.transform.position.z);
            if(lastState != inputData.State)
            {
                Waiting = false;
                Looking = false;
                WaitTime = Time.time + 3;
                lastState = inputData.State;
            }

            switch (inputData.State)
            {

                case SalamanderState.Idle:
                    //Wandering code
                    NavMeshHit hit;
                    NavMeshPath path = new();

                    if (agent.remainingDistance < 0.5)
                    {
                        if(!Waiting)
                        {
                            Waiting = true;
                            WaitTime = Time.time + Random.Range(0.25f, MaxWanderPause);
                        }
                        if(WaitTime <= Time.time)
                        {
                            if(Random.value < 2)
                            {
                                int attempts = 0;
                                bool valid;

                                do
                                {
                                    valid = true;

                                    Vector2 wanderTo = Random.insideUnitCircle * MaxWander;
                                    NavMesh.SamplePosition(new Vector3(wanderTo.x, 0, wanderTo.y) + perceptron.transform.position, out hit, 5, NavMesh.AllAreas);

                                    agent.CalculatePath(hit.position, path);

                                    float pathLength = 0;
                                    if (path.corners.Length > 2)
                                    {
                                        for (int i = 1; i < path.corners.Length; i++)
                                        {
                                            pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                                            if (pathLength > MaxWander)
                                            {
                                                valid = false;
                                                break;
                                            }
                                        }
                                    }

                                    attempts++;
                                    if (attempts >= 100)
                                    {
                                        Debug.Log("FAILED TO FIND VALID WANDER POSITION (in 100 attempts)");
                                        TargetPos = MyPos;
                                        break;
                                    }

                                    if (valid)
                                    {
                                        TargetPos = hit.position;
                                        Waiting = false;
                                    }

                                } while (!valid);
                            }
                           /* else
                            {
                                Looking = true;
                                inputData.looking = true;
                                Vector2 lookDir = new Vector2();
                            }*/
                        }
                    }
                    
                    break;

                case SalamanderState.Attack:
                    //Pew Pew!
                    if (WaitTime <= Time.time)
                    {
                        TargetPos = new Vector3(inputData.PlayerPos.x, perceptron.transform.position.y, inputData.PlayerPos.z);
                    }
                    break;

                case SalamanderState.Flipped:
                    //OH GOD I'VE BEEN HIT WHATEVER SHALL I DO???!?!?!? I KNOW! I SHALL WAIT A FEW SECONDS!

                    //>>>>>>>>>>>>>>>>>>>>>>>>> OH GOD THIS CODE PROBABLY DOESN'T WORK YET!!!!!!! <<<<<<<<<<<<<<<<<<<<<<<<<
                    if(!Waiting)
                    {
                        WaitTime = Time.time + FlippedTime;
                    }
                    if (WaitTime <= Time.time)
                    {
                        flipped = false;
                        agent.isStopped = false;
                    }
                    else
                    {
                        agent.isStopped = true;
                    }
                    break;
            }
            inputData.TargetPos = TargetPos;
        }



        public override void Init()
        {
            agent = perceptron.GetComponentInParent<NavMeshAgent>();
            TargetPos = perceptron.transform.position;
        }

        public override InputMiddleware<SalamanderInput, SalamanderInputEvents.Dispatcher> Clone()
        {
            return new MovementMiddleware
            {
                MinDistance = MinDistance,
                MaxWander = MaxWander,
                MaxWanderPause = MaxWanderPause,
                AttackCooldown = AttackCooldown,
                FlippedTime = FlippedTime,
                WaitTime = WaitTime,
                Waiting = Waiting,
                TargetPos = TargetPos,
                flipped = flipped,
                agent = agent
            };
        }
    }
}