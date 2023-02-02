using System;
using UnityEngine;

namespace Enemies {
	public class EnemyHealthController : MonoBehaviour {
		private int _health;

		public int Health {
			get => _health;
			set {
				_health = value;

				if (value == 0) { OnZero?.Invoke(); } else { OnChange?.Invoke(value); }
			}
		}

		public Action<int> OnChange;
		public Action      OnZero;
	}
}