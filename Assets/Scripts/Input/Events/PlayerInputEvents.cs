using System;
using UnityEngine;

namespace Input.Events {
	public struct PlayerInputEvents : IInputEvents<PlayerInputData, PlayerInputEvents.Dispatcher> {
		public event Action Interact;


		public Dispatcher GetDispatcher(Func<PlayerInputData> inputFunc) {
			Debug.Log($"is OnInteract null: {Interact is null}");
			Dispatcher dispatcher = new Dispatcher(inputFunc, Interact);
			return dispatcher;
		}

		public class Dispatcher : Dispatcher<PlayerInputData> {
			public Dispatcher(Func<PlayerInputData> inputFunc, Action interact) : base(inputFunc) {
				_interact = interact;
				Debug.Log($"are Actions equivalent?: {interact == _interact}");
			}

			private readonly Action _interact;

			public void Interact() {
				Debug.Log("tried to interact");
				//demo event blocking, cant interact while moving
				PlayerInputData inputData = GetInput();
				//if (inputData.movement.magnitude < .2) _invokeInteract?.Invoke();
				_interact.Invoke();
			}
		}
	}
}