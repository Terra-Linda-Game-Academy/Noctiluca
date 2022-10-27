using System;
using System.Collections.Generic;
using UnityEngine;

namespace Input.ConcreteInputProviders {
	[CreateAssetMenu(fileName = "Player Input Provider", menuName = "Input/InputProviders/Player")]
	public class PlayerInputProvider : InputProvider {
		[SerializeReference] public List<PlayerInputMiddleware> middlewares;
		
		public void DebugPrint() {
			Debug.Log("middlewares:");
			
			foreach (PlayerInputMiddleware middleware in middlewares) Debug.Log(middleware);
		}

		public override void ClearBaseObjects() {
			List<PlayerInputMiddleware> toBeDeleted = new List<PlayerInputMiddleware>();

			foreach (PlayerInputMiddleware middleware in middlewares) {
				if (middleware == null) {
					toBeDeleted.Add(middleware);
					Debug.LogWarning("Please add middlewares with the dedicated \"Add Middleware\" button.");
				}
			}

			foreach (PlayerInputMiddleware delete in toBeDeleted) {
				middlewares.Remove(delete);
			}
		}

		private void OnValidate() { ClearBaseObjects(); }
	}
}