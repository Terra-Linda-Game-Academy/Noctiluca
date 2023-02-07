using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.ConcreteInputProviders.Enemy {
	[CreateAssetMenu(fileName = "Mushroom Man Enemy Input Provider", menuName = "Input/InputProviders/Enemy/Mushroom")]
	public class MushroomEnemyInputProvider : InputProvider<MushroomEnemyInput, MushroomEnemyInputEvents,
		MushroomEnemyInputEvents.Dispatcher, MushroomEnemyInputProvider> { }
}