using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.ConcreteInputProviders.Enemy
{
    [CreateAssetMenu(fileName = "Salamander Input Provider", menuName = "Input/InputProviders/Enemy/Salamander")]
    public class SalamanderInputProvider : InputProvider<SalamanderInput, SalamanderInputEvents, SalamanderInputEvents.Dispatcher, SalamanderInputProvider> { }
}