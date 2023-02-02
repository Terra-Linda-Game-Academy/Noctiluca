using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.ConcreteInputProviders.Enemy
{
	[CreateAssetMenu(fileName = "Skink Enemy Input Provider", menuName = "Input/InputProviders/Enemy/Skink")]
	public class SkinkEnemyInputProvider : InputProvider<WalkingEnemyInput, WalkingEnemyInputEvents, WalkingEnemyInputEvents.Dispatcher, WalkingEnemyInputProvider> { }
}