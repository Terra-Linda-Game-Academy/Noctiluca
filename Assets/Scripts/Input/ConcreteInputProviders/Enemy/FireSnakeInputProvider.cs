using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.ConcreteInputProviders.Enemy
{
	[CreateAssetMenu(fileName = "Fire Snake Input Provider", menuName = "Input/InputProviders/Enemy/FireSnake")]
	public class FireSnakeInputProvider : InputProvider<FireSnakeInput, FireSnakeInputEvents, FireSnakeInputEvents.Dispatcher, FireSnakeInputProvider> { }
}
