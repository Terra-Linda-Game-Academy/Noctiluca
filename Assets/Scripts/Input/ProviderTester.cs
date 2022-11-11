using System;
using UnityEngine;

namespace Input {
	public class ProviderTester : MonoBehaviour {
		[SerializeField] private InputProvider<PlayerInputData, PlayerInputEvents> inputProvider;

		public static Action TestAction;

		private void Start() {
			inputProvider.GetInputData();
			TestAction?.Invoke();
			
			inputProvider.events.TestAct += msg => { Debug.Log($"received {msg}"); };

			inputProvider.GetInputData();
			TestAction?.Invoke();
		}
	}
}