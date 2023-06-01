using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.ConcreteInputProviders.Enemy
{
	[CreateAssetMenu(fileName = "Ant Input Provider", menuName = "Input/InputProviders/Enemy/Ant")]
	public class
		AntEnemyInputProvider : InputProvider<AntInput, AntInputEvents, AntInputEvents.Dispatcher,
			AntEnemyInputProvider>
	{ }
}