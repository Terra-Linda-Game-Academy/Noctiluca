using System;
using UnityEngine;
using Util;

namespace Player {
	public class PlayerHealthController : MonoBehaviour {
		[SerializeField] private RuntimeVar<int> healthVar;

		public int Health {
			get => healthVar.Value;
			private set {
				healthVar.Value = value;

				if (value == 0) { OnZero?.Invoke(); } else { OnChange?.Invoke(value); }
			}
		}

		public Action<int> OnChange;
		public Action      OnZero;
	}
}