using Input.Data.Enemy;
using Input.Events.Enemy;
using UnityEngine;

namespace Input.ConcreteInputProviders.Enemy {
	[CreateAssetMenu(fileName = "Draco Input Provider", menuName = "Input/InputProviders/Enemy/Draco")]
	public class
		DracoInputProvider : InputProvider<DracoInput, DracoInputEvents, DracoInputEvents.Dispatcher,
			DracoInputProvider> { }
}