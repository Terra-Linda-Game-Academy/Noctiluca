using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.ConcreteInputProviders.Enemy {
	[CreateAssetMenu(fileName = "Walking Enemy Input Provider", menuName = "Input/InputProviders/Enemy/Walking")]
	public class WalkingEnemyInputProvider : InputProvider<WalkingEnemyInput, WalkingEnemyInputEvents,
		WalkingEnemyInputEvents.Dispatcher, WalkingEnemyInputProvider> { }
}