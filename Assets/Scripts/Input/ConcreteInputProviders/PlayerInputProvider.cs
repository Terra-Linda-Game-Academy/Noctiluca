using UnityEngine;

namespace Input.ConcreteInputProviders {
	[CreateAssetMenu(fileName = "Player Input Provider", menuName = "Input/InputProviders/Player")]
	public class PlayerInputProvider : InputProvider<PlayerInputData> { }
}